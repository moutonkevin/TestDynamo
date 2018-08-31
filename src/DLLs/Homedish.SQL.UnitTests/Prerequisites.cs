using System;
using System.Data;
using System.Data.SqlClient;

namespace Homedish.SQL.UnitTests
{
    public class Prerequisites : IDisposable
    {

        public Prerequisites()
        {
            using (var connection = new SqlConnection(Constants.ConnectionString))
            using (var command = connection.CreateCommand())
            {
                var task = connection.OpenAsync();
                task.Wait();

                command.CommandType = CommandType.Text;
                command.CommandText = @"
                    IF (NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Test'))
                    BEGIN

                        CREATE TABLE[dbo].[Test](
                            [test][nvarchar](50) NULL,
                            [status] [int] NULL,
                            [boolean] [bit] NULL
                        ) ON[PRIMARY];

                        insert into Test (test, status, boolean) values ('hello', 1, 1);
                        insert into Test (test, status, boolean) values ('hello2', 2, 1);
                        insert into Test (test, status, boolean) values ('hello3', 2, 0);
                        insert into Test (test, status, boolean) values ('hello4', 1, 0);
                        insert into Test (test, status, boolean) values ('hello5', 1, 1);
                    
                    END";

                var task2 = command.ExecuteNonQueryAsync();
                task2.Wait();

                command.CommandText = @"
                        IF NOT EXISTS ( SELECT TOP 1 * FROM sysobjects WHERE  [name] = 'usp_test2')
                        BEGIN
                            EXEC('create procedure usp_test2
                            as
                            begin
	                            SELECT * FROM Test
                            end')
                        END";

                var task3 = command.ExecuteNonQueryAsync();
                task3.Wait();

                command.CommandText = @"
                        IF NOT EXISTS ( SELECT TOP 1 * FROM sysobjects WHERE  [name] = 'usp_test')
                        BEGIN
                            EXEC('create procedure usp_test  
                                @status int,  
                                @boolean bit  
                            as  
                            begin  
                                SELECT * FROM Test Where status = @status and boolean = @boolean  
                            end')
                        END";

                var task4 = command.ExecuteNonQueryAsync();
                task4.Wait();
            }
        }

        public void Dispose()
        {
        }
    }
}
