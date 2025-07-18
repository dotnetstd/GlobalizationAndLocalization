// Copyright (c) SecretCollect B.V. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE file in the project root for license information.

using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace SecretCollect.Localization.SqlLocalizer.MigrationStartup
{
    public class Program
    {
        public static void Main2(string[] args)
            => CreateWebHostBuilder(args)
                .Build()
                .Run();

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
