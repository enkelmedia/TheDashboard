import {UmbEntryPointOnInit } from "@umbraco-cms/backoffice/extension-api";
import { UMB_AUTH_CONTEXT } from "@umbraco-cms/backoffice/auth";
import { client } from './backend-api/client.gen.js';
import { registerManifest } from "./manifest.js";

// load up the manifests here
export const onInit: UmbEntryPointOnInit = (host,_extensionRegistry) => {

  host.consumeContext(UMB_AUTH_CONTEXT, async (authContext) => {

    const config = authContext!.getOpenApiConfiguration();

    client.setConfig({
      auth: config.token,
      baseUrl: config.base,
      credentials: config.credentials,
    });

    registerManifest(_extensionRegistry);

  });
};
