import { ManifestTdWidget } from '../../core/td-widget.js'

const widget : ManifestTdWidget = {
	type: "tdWidget",
	alias: "Td.Widget.Counters",
	name: "The Dashboard Widget Counter",
  weight: 20,
  element : ()=>import('./widget-counters.element.ts'),
  meta : {
    width:4,
    height:10
  }
}

export const manifests = [
  widget
]
