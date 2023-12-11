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
            static readonly PersonDBHandler instance = new PersonDBHandler();



            private PersonDBHandler()
            {
                CreateTable();

                Person newp1 = new Person
                {
                    FirstName = "Jude ",
                    LastName = "Bellingham",
                    City = "Madrid",
                    Age = 20
                };
                Person newp2 = new Person
                {
                    FirstName = "Cristiano",
                    LastName = "Ronaldo",
                    City = "Lisbon",
                    Age = 38
                };


                Person newp3 = new Person
                {
                    FirstName = "Masonount",
                    LastName = "MountMason",
                    City = "Manchester",
                    Age = 24
                };


                Person newp4 = new Person
                {
                    FirstName = "Vinicius ",
                    LastName = "Junior",
                    City = "Madrid",
                    Age = 22
                };

                AddPerson(newp1);
                AddPerson(newp2);
                AddPerson(newp3);
                AddPerson(newp4);
            }

            public static PersonDBHandler Instance
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

            public int AddPerson(Person person)
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
                    insertcom.Parameters.AddWithValue("@FirstName", person.FirstName);
                    insertcom.Parameters.AddWithValue("@LastName", person.LastName);
                    insertcom.Parameters.AddWithValue("@City", person.City);
                    insertcom.Parameters.AddWithValue("@Age", person.Age);

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

            public Person GetPerson(int id)
            {
                Person person = new Person();

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
                                person.Id = id2;
                            }
                            person.FirstName = reader["FirstName"].ToString();
                            person.LastName = reader["LastName"].ToString();
                            person.City = reader["City"].ToString();



                            if (Int32.TryParse(reader["Age"].ToString(), out int age))
                            {
                                person.Age = age;
                            }
                        }
                    }
                }
                return person;
            }

            public int UpdatePerson(Person person)

            {
                int row = 0;
                using (SQLiteConnection con = new SQLiteConnection(conString))
                {

                    con.Open();
                    string query = "UPDATE PERSONS SET FirstName= @FirstName, LastName= @LastName," +
                        "City = @City, Age= @Age WHERE Id= @Id";

                    SQLiteCommand updatecom = new SQLiteCommand(query, con);
                    updatecom.Parameters.AddWithValue("@Id", person.Id);
                    updatecom.Parameters.AddWithValue("@FirstName", person.FirstName);
                    updatecom.Parameters.AddWithValue("@LastName", person.LastName);
                    updatecom.Parameters.AddWithValue("@City", person.City);
                    updatecom.Parameters.AddWithValue("@Age", person.Age);

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

            public int DeletePerson(Person person)
            {
                int row = 0;
                using (SQLiteConnection con = new SQLiteConnection(conString))
                {
                    con.Open();
                    string query = "DELETE FROM PERSONS WHERE id= @Id";
                    SQLiteCommand deletecom = new SQLiteCommand(query, con);
                    deletecom.Parameters.AddWithValue("@Id", person.Id);
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

            public List<Person> ReadAllPersons()
            {
                List<Person> listPersons = new List<Person>();
                using (SQLiteConnection con = new SQLiteConnection(conString))
                {
                    con.Open();
                    SQLiteCommand com = new SQLiteCommand("Select * from PERSONS", con);
                    using (SQLiteDataReader reader = com.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            //Create a Person Object
                            Person person = new Person();
                            if (Int32.TryParse(reader["Id"].ToString(), out int id))
                            {
                                person.Id = id;
                            }
                            person.FirstName = reader["FirstName"].ToString();
                            person.LastName = reader["LastName"].ToString();
                            person.City = reader["City"].ToString();



                            if (Int32.TryParse(reader["Age"].ToString(), out int age))
                            {
                                person.Age = age;
                            }
                            listPersons.Add(person);


                        }
                    }
                }
                return listPersons;
            }
        }
    }

