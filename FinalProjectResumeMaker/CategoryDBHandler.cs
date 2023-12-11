using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.Migrations.Model;
using System.Data.SQLite;

namespace FinalProjectResumeMaker
{
    class CategoryDBHandler
    {
            static readonly string conString = ConfigurationManager.ConnectionStrings["MyDB"].ConnectionString;
            static readonly CategoryDBHandler instance = new CategoryDBHandler();



            private CategoryDBHandler()
            {
                CreateTable();

                Category newp1 = new Category
                {
                    FirstName = "Jude ",
                    LastName = "Bellingham",
                    City = "Madrid",
                    Age = 20
                };
                Category newp2 = new Category
                {
                    FirstName = "Cristiano",
                    LastName = "Ronaldo",
                    City = "Lisbon",
                    Age = 38
                };


                Category newp3 = new Category
                {
                    FirstName = "Masonount",
                    LastName = "MountMason",
                    City = "Manchester",
                    Age = 24
                };


                Category newp4 = new Category
                {
                    FirstName = "Vinicius ",
                    LastName = "Junior",
                    City = "Madrid",
                    Age = 22
                };

                AddCategory(newp1);
                AddCategory(newp2);
                AddCategory(newp3);
                AddCategory(newp4);
            }

            public static CategoryDBHandler Instance
            {
                get { return instance; }
            }

            public void CreateTable()
            {
                using (SQLiteConnection con = new SQLiteConnection(conString))
                {
                    con.Open();
                    string drop = "drop table if exists PERSONS;";
                    SQLiteCommand command1 = new SQLiteCommand(drop, con);
                    command1.ExecuteNonQuery();

                    string table = "Create table PERSONS (ID integer primary key," +
                        "FirstName text, " +
                        "LastName text, City text, Age integer);";


                    SQLiteCommand command2 = new SQLiteCommand(table, con);
                    command2.ExecuteNonQuery();



                }
            }

            public int AddCategory(Category category)
            {
                int rows = 0;
                int newId = 0;
                using (SQLiteConnection con = new SQLiteConnection(conString))
                {
                    con.Open();
                    //create a parameterized query
                    string query = "INSERT INTO PERSONS (FirstName, LastName, City, Age) VALUES(@FirstName, " +
                                    "@LastName, @City, @Age)";

                    SQLiteCommand insertcom = new SQLiteCommand(query, con);

                    //pass values to the querry parameters
                    insertcom.Parameters.AddWithValue("@FirstName", category.FirstName);
                    insertcom.Parameters.AddWithValue("@LastName", category.LastName);
                    insertcom.Parameters.AddWithValue("@City", category.City);
                    insertcom.Parameters.AddWithValue("@Age", category.Age);

                    try
                    {
                        rows = insertcom.ExecuteNonQuery();
                        //let get the rowid inserted
                        insertcom.CommandText = " select last_insert_rowid()";
                        Int64 LastRowID64 = Convert.ToInt64(insertcom.ExecuteScalar());
                        //grab the bottom 32-bits as the unique id of the row
                        newId = Convert.ToInt32(LastRowID64);
                    }
                    catch (SQLiteException e)
                    {
                        Console.WriteLine("error generated. Details: " + e.ToString());
                    }
                }
                return newId;
            }

            public Category GetCategory(int id)
            {
                Category category = new Category();

                using (SQLiteConnection con = new SQLiteConnection(conString))
                {
                    con.Open();
                    SQLiteCommand getcom = new SQLiteCommand("Select * from Persons " +
                        "WHERE Id= @Id", con);
                    getcom.Parameters.AddWithValue("@Id", id);

                    using (SQLiteDataReader reader = getcom.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            if (Int32.TryParse(reader["Id"].ToString(), out int id2))
                            {
                                category.Id = id2;
                            }
                            category.FirstName = reader["FirstName"].ToString();
                            category.LastName = reader["LastName"].ToString();
                            category.City = reader["City"].ToString();



                            if (Int32.TryParse(reader["Age"].ToString(), out int age))
                            {
                                category.Age = age;
                            }
                        }
                    }
                }
                return category;
            }

            public int UpdateCategory(Category category)

            {
                int row = 0;
                using (SQLiteConnection con = new SQLiteConnection(conString))
                {

                    con.Open();
                    string query = "UPDATE PERSONS SET FirstName= @FirstName, LastName= @LastName," +
                        "City = @City, Age= @Age WHERE Id= @Id";

                    SQLiteCommand updatecom = new SQLiteCommand(query, con);
                    updatecom.Parameters.AddWithValue("@Id", category.Id);
                    updatecom.Parameters.AddWithValue("@FirstName", category.FirstName);
                    updatecom.Parameters.AddWithValue("@LastName", category.LastName);
                    updatecom.Parameters.AddWithValue("@City", category.City);
                    updatecom.Parameters.AddWithValue("@Age", category.Age);

                    try
                    {
                        row = updatecom.ExecuteNonQuery();
                    }
                    catch (SQLiteException e)
                    {
                        Console.WriteLine("error generated. Details: " + e.ToString());
                    }
                }
                return row;
            }

            public int DeletePerson(Category category)
            {
                int row = 0;
                using (SQLiteConnection con = new SQLiteConnection(conString))
                {
                    con.Open();
                    string query = "DELETE FROM PERSONS WHERE id= @Id";
                    SQLiteCommand deletecom = new SQLiteCommand(query, con);
                    deletecom.Parameters.AddWithValue("@Id", category.Id);
                    try
                    {
                        row = deletecom.ExecuteNonQuery();
                    }
                    catch (SQLiteException e)
                    {
                        Console.WriteLine("Error geenrated detials:" + e.ToString());
                    }
                    return row;
                }

            }

            public List<Category> ReadAllCategory()
            {
                List<Category> listCategories = new List<Category>();
                using (SQLiteConnection con = new SQLiteConnection(conString))
                {
                    con.Open();
                    SQLiteCommand com = new SQLiteCommand("Select * from PERSONS", con);
                    using (SQLiteDataReader reader = com.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            //Create a Person Object
                            Category category = new Category();
                            if (Int32.TryParse(reader["Id"].ToString(), out int id))
                            {
                                category.Id = id;
                            }
                            category.FirstName = reader["FirstName"].ToString();
                            category.LastName = reader["LastName"].ToString();
                            category.City = reader["City"].ToString();



                            if (Int32.TryParse(reader["Age"].ToString(), out int age))
                            {
                                category.Age = age;
                            }
                            listCategories.Add(category);


                        }
                    }
                }
                return listCategories;
            }
        }
    }

