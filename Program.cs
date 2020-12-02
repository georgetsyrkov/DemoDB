using System;
using System.Data.SQLite;

namespace db_demo
{
    class Program
    {
        static void Main(string[] args)
        {                        
            SQLiteConnection conn =
                new SQLiteConnection("Data Source=mydb.db;Version=3;");

            conn.Open();

            if (args.Length > 0)
            {
                if (args[0] == "create")
                {
                    CreateTables(conn);
                }
            }

            Console.WriteLine("Нажмите 'l' для вывода всех данных или 'a' для добавления");
            ConsoleKeyInfo key = Console.ReadKey();

            if (key.KeyChar == 'l')
            {
                GetAllData(conn);
            }
            else if (key.KeyChar == 'a')
            {
                Console.WriteLine("");
                Console.WriteLine("Введите новые данные:");
                string newData = Console.ReadLine();
                AddNewData(conn, newData);
            }

            conn.Close();
        }

        private static void CreateTables(SQLiteConnection conn)
        {
            string sql_table_user = "create table USERS (ID NUMBER NOT NULL, NAME VARCHAR(255) NULL, PASSWORD VARCHAR(255) NULL)";
            string sql_table_data = "create table DATA (ID NUMBER NOT NULL, VALUE VARCHAR(255) NULL)";

            SQLiteCommand cmd_create_user = new SQLiteCommand(sql_table_user, conn);
            try { cmd_create_user.ExecuteNonQuery(); } catch (Exception ex) { Console.WriteLine($"Ошибка создания таблицы: {ex.ToString()}"); }

            SQLiteCommand cmd_create_data = new SQLiteCommand(sql_table_data, conn);
            try { cmd_create_data.ExecuteNonQuery(); } catch (Exception ex) { Console.WriteLine($"Ошибка создания таблицы: {ex.ToString()}"); }


            string sql_insert = "insert into USERS (ID, NAME, PASSWORD) values ( 1, 'root', 'god'); "+
                                "insert into USERS (ID, NAME, PASSWORD) values ( 2, 'admin', 'verysecretpassword'); " + 
                                "insert into USERS (ID, NAME, PASSWORD) values ( 3, 'user', '123456'); ";
            SQLiteCommand cmd_insert = new SQLiteCommand(sql_insert, conn);
            try { cmd_insert.ExecuteNonQuery(); } catch (Exception ex) { Console.WriteLine($"Ошибка создания таблицы: {ex.ToString()}"); }
        }

        private static void GetAllData(SQLiteConnection conn)
        {
            string sql = "select * from DATA";

            SQLiteCommand cmd = new SQLiteCommand(sql, conn);
            SQLiteDataReader dr = cmd.ExecuteReader();

            string header = string.Empty;
            for (int i = 0; i < dr.FieldCount; i++)
            {
                header += dr.GetName(i) + "\t";
            }
            Console.WriteLine("");
            Console.WriteLine(header);

            while (dr.Read())
            {
                string line = string.Empty;
                for (int i = 0; i < dr.FieldCount; i++)
                {
                    line += dr.GetValue(i) + "\t";
                }
                Console.WriteLine(line);
            }
            dr.Close();
        }

        private static void AddNewData(SQLiteConnection conn, string newData)
        {
            int max_id = 0;
            string sql_find_max_id = "select count(ID) from DATA";
            SQLiteCommand cmd_find_max_id = new SQLiteCommand(sql_find_max_id, conn);
            SQLiteDataReader dr_find_max_id = cmd_find_max_id.ExecuteReader();
            while (dr_find_max_id.Read())
            {
                max_id = dr_find_max_id.GetInt32(0);
            }
            dr_find_max_id.Close();

            max_id = max_id + 1;

            string sql_insert = 
              "insert into DATA (ID, VALUE) values "+
              "(" + max_id + ", '" + newData + "')";
              
            SQLiteCommand cmd_insert = new SQLiteCommand(sql_insert, conn);
            cmd_insert.ExecuteNonQuery();
        }
    }
}
