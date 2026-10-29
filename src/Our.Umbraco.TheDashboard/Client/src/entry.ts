import {UmbEntryPointOnInit } from "@umbraco-cms/backoffice/extension-api";
import { UMB_AUTH_CONTEXT } from "@umbraco-cms/backoffice/auth";
import { client } from './backend-api/client.gen.js';
import { registerManifest } from "./manifest.js";

// load up the manifests here
export const onInit: UmbEntryPointOnInit = (host,_extensionRegistry) => {

  host.consumeContext(UMB_AUTH_CONTEXT, async (authContext) => {

    authContext!.configureClient(client);

    registerManifest(_extensionRegistry);

  });
};
