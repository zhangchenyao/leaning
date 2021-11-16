using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ioctest
{
    class Class2
    {
        public object sss()
        {
            Type typeToCreate = typeof(Employee);
            ConstructorInfo ctor = typeToCreate.GetConstructor(System.Type.EmptyTypes);
            return  ctor.Invoke(null) as Employee;
        }

        public object CeratObject()
        {
            return  Activator.CreateInstance<Employee>();
        }
        public object CreatObject()
        {
            IServiceCollection services = new ServiceCollection();

            services.AddTransient<Employee>();

            IServiceProvider provider = services.BuildServiceProvider();

            Employee employee = provider.GetService<Employee>();
            return employee;
        }

    }
}
