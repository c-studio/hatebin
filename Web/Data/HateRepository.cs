﻿using Interactive.HateBin.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Interactive.HateBin.Data
{
    public class HateRepository
    {
        private MySqlConnection Connection => new MySqlConnection(Configuration.DefaultConnectionString);

        public Hate Save(Hate item)
        {
            item.Created = DateTime.Now;
            using (var conn = Connection)
            {
                conn.Open();

                var transaction = conn.BeginTransaction();

                var command = new MySqlCommand("INSERT INTO hate (created, network, networkId, text) VALUES (@Created, @Network, @NetworkId, @Text);", conn, transaction);

                command.Parameters.AddWithValue("@Created", item.Created);
                command.Parameters.AddWithValue("@Network", item.Network);
                command.Parameters.AddWithValue("@NetworkId", item.NetworkId);
                command.Parameters.AddWithValue("@Text", item.Text);
                command.ExecuteNonQuery();

                var idCommand = new MySqlCommand("SELECT LAST_INSERT_ID();", conn, transaction);
                var id = idCommand.ExecuteScalar();
                item.Id = Convert.ToInt32(id);

                foreach (var category in item.Categories)
                {
                    var categoryCommand = new MySqlCommand("INSERT INTO `hate-categories` (hateId, category) VALUES (@HateId, @Category);", conn, transaction);
                    categoryCommand.Parameters.AddWithValue("@HateId", item.Id);
                    categoryCommand.Parameters.AddWithValue("@Category", category);
                    categoryCommand.ExecuteNonQuery();
                }

                transaction.Commit();
            }
            return item;
        }

        public Hate Get(int id)
        {
            Hate result = null;
            using (var conn = Connection)
            {
                conn.Open();
                var command = new MySqlCommand("SELECT * FROM hate WHERE id=@Id", conn);
                command.Parameters.AddWithValue("@Id", id);
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        result = new Hate
                        {
                            Id = id,
                            Created = reader.GetDateTime("created"),
                            Network = reader.GetString("network"),
                            NetworkId = reader.GetInt64("networkId"),
                            Text = reader.GetString("text"),
                        };
                    }
                    else
                    {
                        return null;
                    }
                }

                var categoryCommand = new MySqlCommand("SELECT * FROM `hate-categories` WHERE hateId=@Id", conn);
                categoryCommand.Parameters.AddWithValue("@Id", id);

                using (var categoryReader = categoryCommand.ExecuteReader())
                {
                    result.Categories = new List<string>();

                    while (categoryReader.Read())
                    {
                        result.Categories.Add(categoryReader.GetString("category"));
                    }
                }
            }
            return result;
        }

        public int GetCount()
        {
            using (var conn = Connection)
            {
                conn.Open();
                var command = new MySqlCommand("SELECT COUNT(*) FROM hate", conn);
                var count = command.ExecuteScalar();
                return Convert.ToInt32(count);
            }
        }

        public IEnumerable<Hate> GetList(int id = 0,  int count = 21, PageDirection direction = PageDirection.Forward)
        {
            var result = new List<Hate>();
            using (var conn = Connection)
            {
                conn.Open();

                MySqlCommand command;
                if(id == 0)
                {
                    command = new MySqlCommand("SELECT * FROM hate ORDER BY Id DESC LIMIT @Count", conn);
                    command.Parameters.AddWithValue("@Count", count);
                }
                else
                {
                    if (direction == PageDirection.Forward)
                    {
                        command = new MySqlCommand("SELECT * FROM hate WHERE id < @Id ORDER BY Id DESC LIMIT @Count", conn);
                        command.Parameters.AddWithValue("@Id", id);
                        command.Parameters.AddWithValue("@Count", count);
                    }
                    else
                    {
                        command = new MySqlCommand("SELECT * FROM hate WHERE id > @Id ORDER BY Id ASC LIMIT @Count", conn);
                        command.Parameters.AddWithValue("@Id", id);
                        command.Parameters.AddWithValue("@Count", count);
                    }
                }

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var item = new Hate
                        {
                            Id = reader.GetInt32("id"),
                            Created = reader.GetDateTime("created"),
                            Network = reader.GetString("network"),
                            NetworkId = reader.GetInt64("networkId"),
                            Text = reader.GetString("text"),
                        };
                        result.Add(item);
                    }
                }

                foreach (var item in result)
                {
                    var categoryCommand = new MySqlCommand("SELECT * FROM `hate-categories` WHERE hateId=@Id", conn);
                    categoryCommand.Parameters.AddWithValue("@Id", item.Id);
                    using (var categoryReader = categoryCommand.ExecuteReader())
                    {
                        item.Categories = new List<string>();
                        while (categoryReader.Read())
                        {
                            item.Categories.Add(categoryReader.GetString("category"));
                        }
                    }
                }
            }
            return result;
        }

        public HateStats GetStats()
        {
            using (var conn = Connection)
            {
                conn.Open();
                var command = new MySqlCommand("SELECT (SELECT COUNT(*) FROM `hate`) as hate," +
                                               "(SELECT COUNT(*) FROM `hate-categories` WHERE category = 'Racist') as racist," +
                                               "(SELECT COUNT(*) FROM `hate-categories` WHERE category = 'Sexist') as sexist," +
                                               "(SELECT COUNT(*) FROM `hate-categories` WHERE category = 'Ad Hominem') as ad_hominem," +
                                               "(SELECT COUNT(*) FROM `hate-categories` WHERE category NOT IN ('Ad Hominem', 'Racist', 'Sexist')) as other", conn);
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        var item = new HateStats
                        {
                            HateCount = reader.GetInt32("hate"),
                            RacistCount = reader.GetInt32("racist"),
                            SexistCount = reader.GetInt32("sexist"),
                            AdHominemCount = reader.GetInt32("ad_hominem"),
                            OtherCount = reader.GetInt32("other")
                        };
                        return item;
                    }
                    return null;
                }
            }
        }
    }
}