using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using yournotes.Models;

namespace yournotes2 {
   public class Startup {
      public Startup(IConfiguration configuration) {
         Configuration = configuration;
      }

      public IConfiguration Configuration { get; }

      public IServiceProvider ConfigureServices(IServiceCollection services) {
         services.AddDbContext<Db>(opt => opt.UseInMemoryDatabase("db"));
         services.AddCors();
         services.AddMvc();
         var serviceProvider = services.BuildServiceProvider();
         return serviceProvider;
      }

      public void Configure(IApplicationBuilder app, IHostingEnvironment env) {
         if (env.IsDevelopment()) {
            app.UseDeveloperExceptionPage();
         }
         
         app.UseCors(builder => builder.AllowAnyOrigin());

         app.UseMvc();
         Db contextDb = app.ApplicationServices.GetService<Db>();
         AddTestData(contextDb);
      }
      
      private static void AddTestData(Db context) {
         var testUser = new User
         {
            Id = 0,
            Email = "example@gmail.com",
            Password = "root"
         };
 
         context.Users.Add(testUser);
 
         var testNote = new Note
         {
            Id = 0,
            User = testUser,
            Title = "Your first note!",
            Text = "Here is your first content!"
         };
 
         context.Notes.Add(testNote);
 
         context.SaveChanges();
      }
   }
}