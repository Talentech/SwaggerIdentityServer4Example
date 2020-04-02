using System;

namespace SwaggerIdentityServer4Example.V1.Models
{
    public class DemoModel
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; }
    }
}
