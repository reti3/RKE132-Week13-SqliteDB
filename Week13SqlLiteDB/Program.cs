using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Reflection.PortableExecutable;
using System.Data.SQLite;
using System.Reflection.Emit;
using System.Reflection.Metadata;
using Microsoft.Data.Sqlite; //töötab selle paketiga; system.data.sqlite annab errori: unable to load shared library

ReadData(CreateConnection());
//InsertCustomer(CreateConnection()); //kliendi lisamine
//RemoveCustomer(CreateConnection());
FindCustomer(CreateConnection());


static SqliteConnection CreateConnection() //tagastab objekti SQLiteConnection
{
    SqliteConnection connection = new SqliteConnection("Data Source=mydb.db"); //ühendus andmebaasiga

    try
    {
        connection.Open(); // proovib andmebaasi lahti teha
        Console.WriteLine("DB found");
    }

    catch
    {
        Console.WriteLine("DB not found");
    }

    return connection;
}


static void ReadData(SqliteConnection myConnection)
{
    Console.Clear();
    SqliteDataReader reader;
    SqliteCommand command;

    command = myConnection.CreateCommand();
    command.CommandText = "SELECT rowid, *FROM customer "; //tühikud on OLULISED

    reader = command.ExecuteReader(); //käsk, hakka lugema andmeid ja salvesta reader-sse

    while (reader.Read()) //käib ringi, kuni tabelis saavad read otsa
    {
        string readerStringRowId = reader.GetString(0);
        // string readerStringRowId = reader["rowid"].ToString(); - teine võimalus, kuidas andmeid nt veerust saada
        string readerStringFirstName = reader.GetString(1);
        string readerStringLastName = reader.GetString(2);
        string readerStringDoB = reader.GetString(3);

        Console.WriteLine($" RowId: {readerStringRowId}, Full name: {readerStringFirstName} {readerStringLastName}; DoB: {readerStringDoB}");

    }

    myConnection.Close();
}

static void InsertCustomer(SqliteConnection myConnection)
{
    SqliteCommand command;
    string fName, lName, dob;

    Console.WriteLine("Enter first name: ");
    fName = Console.ReadLine();
    Console.WriteLine("Enter last name: ");
    lName = Console.ReadLine();
    Console.WriteLine("Enter date of birth (mm-dd-yyyy): ");
    dob = Console.ReadLine();

    command = myConnection.CreateCommand();
    command.CommandText = $"INSERT INTO customer (firstName, lastName, dateOfBirth) " +
    $"VALUES ('{fName}', '{lName}', '{dob}')";

    int rowInserted = command.ExecuteNonQuery();
    Console.WriteLine($"Row inserted: {rowInserted}"); //vastus, mitu rida on lisatud



    ReadData(myConnection);
}
    

static void RemoveCustomer(SqliteConnection myConnection)
{
    
    SqliteCommand command;
    string idToDelete;
    Console.WriteLine("Enter an id to delete a customer: ");
    idToDelete = Console.ReadLine();

    command = myConnection.CreateCommand();
    command.CommandText = $"DELETE FROM customer WHERE rowid = {idToDelete}";
    //{idToDelete}";


    int rowRemoved = command.ExecuteNonQuery();
    Console.WriteLine($"{rowRemoved} was removed from the customer table.");

    ReadData(myConnection);

}



static void FindCustomer(SqliteConnection myConnection)
{
    SqliteDataReader reader;
    SqliteCommand command;
    string ToFindCustomer;
    Console.WriteLine("Enter a first name of customer you want to find: ");
    ToFindCustomer = Console.ReadLine();

    command = myConnection.CreateCommand();
    command.CommandText = "SELECT firstName, lastName FROM customer WHERE firstName = @ToFindCustomer";
    command.Parameters.AddWithValue("@ToFindCustomer", ToFindCustomer);


    reader = command.ExecuteReader();

    while (reader.Read())
    {
        string firstName = reader.GetString(0);
        string lastName = reader.GetString(1);
        Console.WriteLine($"firstName: {firstName}, lastName: {lastName}");
    }
    reader.Close();
}

