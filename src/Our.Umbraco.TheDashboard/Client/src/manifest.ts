import type { ManifestLocalization, UmbBackofficeExtensionRegistry } from "@umbraco-cms/backoffice/extension-registry";
import { manifests as dashboardManifests } from "./dashboard/manifest.js";

const translationManifests : Array<ManifestLocalization> = [
	{
		type: "localization",
		alias: "TheDasboard.Localize.En_US",
		name: "The Dashboard Localization English (United States)",
		meta: {
			"culture": "en-us"
		},
		js : ()=> import('./localization/en-us.js')
	},
	{
		type: "localization",
		alias: "TheDasboard.Localize.Sv_SE",
		name: "The Dashboard Localization Swedish (Sweden)",
		meta: {
			"culture": "sv-se"
		},
		js : ()=> import('./localization/sv-se.js')
	},
  {
		type: "localization",
		alias: "TheDasboard.Localize.Da_DK",
		name: "The Dashboard Localization Danish (Denmark)",
		meta: {
			"culture": "da-dk"
		},
		js : ()=> import('./localization/da-dk.js')
	},
  {
		type: "localization",
		alias: "TheDasboard.Localize.Es_ES",
		name: "The Dashboard Localization Spanish (Spain)",
		meta: {
			"culture": "es-es"
		},
		js : ()=> import('./localization/es-es.js')
	},
  {
		type: "localization",
		alias: "TheDasboard.Localize.Hr_HR",
		name: "The Dashboard Localization Croatian (Croatia)",
		meta: {
			"culture": "hr-hr"
		},
		js : ()=> import('./localization/hr-hr.js')
	},
  {
		type: "localization",
		alias: "TheDasboard.Localize.Nb_NO",
		name: "The Dashboard Localization Norwegian Bokmål (Norway)",
		meta: {
			"culture": "nb-no"
		},
		js : ()=> import('./localization/nb-no.js')
	},
  {
		type: "localization",
		alias: "TheDasboard.Localize.Nl_NL",
		name: "The Dashboard Localization Dutch (Netherlands)",
		meta: {
			"culture": "nl-nl"
		},
		js : ()=> import('./localization/nl-nl.js')
	}
]

export function registerManifest(registry : UmbBackofficeExtensionRegistry) {

  console.log('register extensions');

    registry.registerMany([
		...dashboardManifests,
    ...translationManifests
	]);
}
