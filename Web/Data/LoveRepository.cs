using Interactive.HateBin.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace Interactive.HateBin.Data
{
    public class LoveRepository
    {
        private MySqlConnection Connection => new MySqlConnection(Configuration.DefaultConnectionString);

        public Love Save(Love item)
        {
            item.Updated = DateTime.Now;
            if (item.Id == 0)
            {
                item.Created = item.Updated;
                item.Sent = 0;
            }

            using (var conn = Connection)
            {
                conn.Open();
                var transaction = conn.BeginTransaction();

                var command = new MySqlCommand("INSERT INTO love (id, created, updated, email, reason, sent) VALUES (@Id, @Created, @Updated, @Email, @Reason, @Sent) " +
                                               "ON DUPLICATE KEY UPDATE updated = VALUES(updated), email=VALUES(email), reason=VALUES(reason), sent=VALUES(sent);", conn, transaction);

                command.Parameters.AddWithValue("@Id", item.Id);
                command.Parameters.AddWithValue("@Created", item.Created);
                command.Parameters.AddWithValue("@Updated", item.Updated);
                command.Parameters.AddWithValue("@Email", item.Email);
                command.Parameters.AddWithValue("@Reason", item.Reason);
                command.Parameters.AddWithValue("@Sent", item.Sent);
                command.ExecuteNonQuery();

                if (item.Id == 0)
                {
                    var idCommand = new MySqlCommand("SELECT LAST_INSERT_ID();", conn, transaction);
                    var id = idCommand.ExecuteScalar();
                    item.Id = Convert.ToInt32(id);
                }
                transaction.Commit();
            }

            return item;
        }

        public Love Get(int id)
        {
            using (var conn = Connection)
            {
                conn.Open();
                var command = new MySqlCommand("SELECT * FROM love WHERE id=@Id", conn);
                command.Parameters.AddWithValue("@Id", id);
                var reader = command.ExecuteReader();

                if (reader.Read())
                {
                    var item = ParseLove(reader);
                    return item;
                }
                return null;
            }
        }

        public int GetCount()
        {
            using (var conn = Connection)
            {
                conn.Open();
                var command = new MySqlCommand("SELECT COUNT(*) FROM love", conn);
                var count = command.ExecuteScalar();
                return Convert.ToInt32(count);
            }
        }

        public IEnumerable<Love> GetAllPending()
        {
            var result = new List<Love>();
            using (var conn = Connection)
            {
                conn.Open();
                var command = new MySqlCommand("SELECT * FROM love WHERE sent <= 5;" , conn);
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var item = ParseLove(reader);
                    result.Add(item);
                }
            }
            return result;
        }

        public IEnumerable<Love> GetList(int id = 0, int count = 21, PageDirection direction = PageDirection.Forward)
        {
            var result = new List<Love>();
            using (var conn = Connection)
            {
                conn.Open();
                MySqlCommand command;

                if (id == 0)
                {
                    command = new MySqlCommand("SELECT * FROM love ORDER BY Id DESC LIMIT @Count;", conn);
                    command.Parameters.AddWithValue("@Count", count);
                }
                else
                {
                    if (direction == PageDirection.Forward)
                    {
                        command = new MySqlCommand("SELECT * FROM love WHERE id < @Id ORDER BY Id DESC LIMIT @Count", conn);
                        command.Parameters.AddWithValue("@Id", id);
                        command.Parameters.AddWithValue("@Count", count);
                    }
                    else
                    {
                        command = new MySqlCommand("SELECT * FROM love WHERE id > @Id ORDER BY Id ASC LIMIT @Count", conn);
                        command.Parameters.AddWithValue("@Id", id);
                        command.Parameters.AddWithValue("@Count", count);
                    }
                }

                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var item = ParseLove(reader);
                    result.Add(item);
                }
            }
            return result;
        }

        private static Love ParseLove(MySqlDataReader reader)
        {
            return new Love
            {
                Id = reader.GetInt32("id"),
                Created = reader.GetDateTime("created"),
                Updated = reader.GetDateTime("updated"),
                Email = reader.GetString("email"),
                Reason = reader.GetString("reason"),
                Sent = reader.GetInt32("sent")
            };
        }
    }
}