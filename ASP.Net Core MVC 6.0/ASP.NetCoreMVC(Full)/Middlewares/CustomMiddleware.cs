namespace ASP.NetCoreMVCEx1.Middlewares
{
    public class CustomMiddleware
    {
        private readonly RequestDelegate _next;
        public CustomMiddleware(RequestDelegate next)
        {
            _next = next ;
        }
        public async Task InvokeAsync(HttpContext context)
        {  
            string folder = @"data\";
            string fileName = "LogMiddleware.txt";
            string fullPath = folder + fileName;
            
            var currentTime = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            var scheme = (context.Request.Scheme+"://");
            var host = context.Request.Host;
            var path = context.Request.Path;
            var queryString = context.Request.QueryString;
            var reader = new StreamReader(context.Request.Body);
            var body = await reader.ReadToEndAsync();

            //Get all query name & value only
            var queryPart =queryString.ToString()==""?null: queryString.ToString().Substring(1).Split("&").ToList();
            var queryKeyValue = new Dictionary<string, string>();
            if(queryPart != null)
            {
                queryPart.ForEach(query=>{
                    var part = query.Split("=").ToArray();
                    queryKeyValue.Add(part[0],part[1]);
                });
            }
            
            var fullDomain = $"\n{scheme}://{host}{path}{queryString} \n  {currentTime}\n Scheme: {scheme}|| Host: {host}|| Path: {path}||Query Full: {queryString}||Query key: {String.Join(",",queryKeyValue.Keys)}||Query value: {String.Join(",",queryKeyValue.Values)} || Body:{body}\n";
            await File.WriteAllTextAsync(fullPath, fullDomain);
            
            Console.WriteLine(fullDomain);
            foreach(var query in queryKeyValue)
            {
                 Console.WriteLine("Key: {0} - Value: {1}",query.Key,query.Value);
            }
            await _next(context);
        }
    }
}