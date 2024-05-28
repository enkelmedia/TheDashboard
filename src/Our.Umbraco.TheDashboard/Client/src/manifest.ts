import type { ManifestLocalization, UmbBackofficeExtensionRegistry } from "@umbraco-cms/backoffice/extension-registry";
import { manifests as dashboardManifests } from "./dashboard/manifest.js";

const translationManifests : Array<ManifestLocalization> = [
	{
		type: "localization",
		alias: "TheDasboard.Localize.En_US",
		name: "English (United States)",
		meta: {
			"culture": "en-us"
		},
		js : ()=> import('./localization/en-us.js')
	},
	{
		type: "localization",
		alias: "TheDasboard.Localize.Sv_SE",
		name: "Swedish (Sweden)",
		meta: {
			"culture": "sv-se"
		},
		js : ()=> import('./localization/sv-se.js')
	},
]

export function registerManifest(registry : UmbBackofficeExtensionRegistry) {

  console.log('register extensions');

    registry.registerMany([
		...dashboardManifests,
    ...translationManifests
	]);
}
