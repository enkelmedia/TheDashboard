import { defineConfig,defaultPlugins } from '@hey-api/openapi-ts';

export default defineConfig({
	input: 'http://localhost:24760/umbraco/openapi/the-dashboard.json',
	output: {
		path: './src/backend-api',
	},
	plugins: [
    ...defaultPlugins,
		{
			name: '@hey-api/typescript',
			enums: 'typescript'
		},
		{
			name: '@hey-api/sdk',
			operations: {
				strategy: 'byTags',
				container: 'class',
				containerName: '{{name}}Resource'
			}
		}
	]
});
