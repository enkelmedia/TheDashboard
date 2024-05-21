import { defineConfig } from '@hey-api/openapi-ts';

export default defineConfig({
  input: 'http://localhost:24760/umbraco/swagger/the-dashboard/swagger.json',
  output: 'src/backend-api',
  //format : false,
  //enums : 'javascript', // Typescript not recommended https://heyapi.vercel.app/openapi-ts/configuration.html#enums
  schemas : false,
  //lint : false,
  //debug: true,
  services: {
    name: '{{name}}Resource'
  }
});
