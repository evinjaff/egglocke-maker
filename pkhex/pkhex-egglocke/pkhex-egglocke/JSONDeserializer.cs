using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;


namespace pkhex_egglocke
{
    public abstract class Resource
    {
        public static JsonSerializerOptions SerializerOptions = new JsonSerializerOptions()
        {
            PropertyNameCaseInsensitive = true
        };

        public async Task<T> DeserializeAsync<T>(Stream stream)
        {
            return await DeserializeAsync<T>(stream, CancellationToken.None);
        }

        public async Task<T> DeserializeAsync<T>(Stream stream, CancellationToken token)
        {
            return await JsonSerializer.DeserializeAsync<T>(stream, SerializerOptions, token);
        }

    }
        
}
