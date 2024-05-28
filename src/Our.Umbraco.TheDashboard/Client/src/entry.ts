import type { UmbEntryPointOnInit } from '@umbraco-cms/backoffice/extension-api';
import { UMB_AUTH_CONTEXT } from '@umbraco-cms/backoffice/auth';
import { registerManifest } from './manifest.js';
import { OpenAPI } from './backend-api/index.js';

export const onInit: UmbEntryPointOnInit = (host, extensionRegistry) => {

  host.consumeContext(UMB_AUTH_CONTEXT,(auth)=> {

      const config = auth.getOpenApiConfiguration();

      OpenAPI.BASE = config.base;
      OpenAPI.WITH_CREDENTIALS = config.withCredentials;
      OpenAPI.CREDENTIALS = config.credentials;
      OpenAPI.TOKEN = config.token;

  });

  registerManifest(extensionRegistry);

};
