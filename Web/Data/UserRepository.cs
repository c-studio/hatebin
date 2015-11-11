using Interactive.HateBin.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Interactive.HateBin.Data
{
    public class UserRepository
    {
        private MySqlConnection Connection => new MySqlConnection(Configuration.DefaultConnectionString);

        public User Save(User item)
        {
            using (var conn = Connection)
            {
                conn.Open();
                var transaction = conn.BeginTransaction();

                var command = new MySqlCommand("INSERT INTO users (id, name, email, password, roles) VALUES (@Id, @Name, @Email, @Password, @Roles) " +
                                               "ON DUPLICATE KEY UPDATE name=VALUES(name), email=VALUES(email), password=VALUES(password), roles=VALUES(roles);", conn, transaction);

                command.Parameters.AddWithValue("@Id", item.Id);
                command.Parameters.AddWithValue("@Name", item.Name);
                command.Parameters.AddWithValue("@Email", item.Email);
                command.Parameters.AddWithValue("@Password", item.Password);
                command.Parameters.AddWithValue("@Roles", string.Join(",", item.Roles));
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

        public User GetById(int id)
        {
            User result = null;
            using (var conn = Connection)
            {
                conn.Open();
                var command = new MySqlCommand("SELECT * FROM users WHERE id=@Id", conn);
                command.Parameters.AddWithValue("@Id", id);
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        result = new User
                        {
                            Id = reader.GetInt32("id"),
                            Name = reader.GetString("name"),
                            Email = reader.GetString("email"),
                            Password = reader.GetString("password"),
                            Roles = reader.GetString("roles").Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries).ToList()
                        };
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            return result;
        }

        public User GetByEmail(string email)
        {
            User result = null;
            using (var conn = Connection)
            {
                conn.Open();
                var command = new MySqlCommand("SELECT * FROM users WHERE email=@Email", conn);
                command.Parameters.AddWithValue("@Email", email);
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        result = new User
                        {
                            Id = reader.GetInt32("id"),
                            Name = reader.GetString("name"),
                            Email = reader.GetString("email"),
                            Password = reader.GetString("password"),
                            Roles = reader.GetString("roles").Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList()
                        };
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            return result;
        }

        public IEnumerable<User> GetList(int id = 0, int count = 21, PageDirection direction = PageDirection.Forward)
        {
            var result = new List<User>();
            using (var conn = Connection)
            {
                conn.Open();
                MySqlCommand command;
                if (id == 0)
                {
                    command = new MySqlCommand("SELECT * FROM users ORDER BY Id DESC LIMIT @Count", conn);
                    command.Parameters.AddWithValue("@Count", count);
                }
                else
                {
                    if (direction == PageDirection.Forward)
                    {
                        command = new MySqlCommand("SELECT * FROM users WHERE id < @Id ORDER BY Id DESC LIMIT @Count", conn);
                        command.Parameters.AddWithValue("@Id", id);
                        command.Parameters.AddWithValue("@Count", count);
                    }
                    else
                    {
                        command = new MySqlCommand("SELECT * FROM users WHERE id > @Id ORDER BY Id ASC LIMIT @Count", conn);
                        command.Parameters.AddWithValue("@Id", id);
                        command.Parameters.AddWithValue("@Count", count);
                    }
                }


                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var item = new User
                        {
                            Id = reader.GetInt32("id"),
                            Name = reader.GetString("name"),
                            Email = reader.GetString("email"),
                            Password = reader.GetString("password"),
                            Roles = reader.GetString("roles").Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList()
                        };
                        result.Add(item);
                    }
                }
            }
            return result;
        } 
    }
}