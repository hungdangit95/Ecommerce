﻿using Npgsql;

namespace Discount.Grpc.Extension
{
    public static class WebExtesion
    {
        public static void MigrationData<TContext>(this WebApplication app)
        {
                 using (var scope = app.Services.CreateScope())
                try
                {
                    var services = scope.ServiceProvider;
                    var configuration = services.GetRequiredService<IConfiguration>();
                    var logger = services.GetRequiredService<ILogger<TContext>>();

                    try
                    {
                        logger.LogInformation("Migrating postresql database.");

                        using var connection = new NpgsqlConnection("Server=discountdb;Port=5432;Database=DiscountDb;User Id=admin;Password=admin1234;");
                        connection.Open();

                        using var command = new NpgsqlCommand
                        {
                            Connection = connection
                        };

                        command.CommandText = "DROP TABLE IF EXISTS Coupon";
                        command.ExecuteNonQuery();

                        command.CommandText = @"CREATE TABLE Coupon(Id SERIAL PRIMARY KEY, 
                                                                ProductName VARCHAR(24) NOT NULL,
                                                                Description TEXT,
                                                                Amount INT)";
                        command.ExecuteNonQuery();

                        command.CommandText = "INSERT INTO Coupon(ProductName, Description, Amount) VALUES('IPhone X', 'IPhone Discount', 150);";
                        command.ExecuteNonQuery();

                        command.CommandText = "INSERT INTO Coupon(ProductName, Description, Amount) VALUES('Samsung 10', 'Samsung Discount', 100);";
                        command.ExecuteNonQuery();

                        logger.LogInformation("Migrated postresql database.");
                    }
                    catch (NpgsqlException ex)
                    {
                        logger.LogError(ex, "An error occurred while migrating the postresql database");

                        //if (retryForAvailability < 50)
                        //{
                        //    retryForAvailability++;
                        //    System.Threading.Thread.Sleep(2000);
                        //    MigrateDatabase<TContext>(host, retryForAvailability);
                        //}
                    }

                }
                catch(Exception ex)
                {
                    throw;
                }
        }
    }
}
