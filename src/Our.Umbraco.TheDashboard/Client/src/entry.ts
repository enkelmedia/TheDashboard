import type { UmbEntryPointOnInit } from "@umbraco-cms/backoffice/extension-api";
import { UMB_AUTH_CONTEXT } from "@umbraco-cms/backoffice/auth";
import { client } from './backend-api/client.gen.js';
import { registerManifest } from "./manifest.js";

export const onInit: UmbEntryPointOnInit = async (host, extensionRegistry) => {
  const authContext = await host.getContext(UMB_AUTH_CONTEXT);
  if (!authContext) {
    console.warn("UMB_AUTH_CONTEXT is unavailable. The Dashboard API client cannot authenticate.");
    return;
  }

  authContext.configureClient(client);
  registerManifest(extensionRegistry);
};
