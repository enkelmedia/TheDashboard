import { ManifestTdWidget } from '../../core/td-widget.js'

const widget : ManifestTdWidget = {
	type: "tdWidget",
	alias: "Td.Widget.PendingContent",
	name: "The Dashboard Widget Pending Content",
  weight: 100,
  element : ()=>import('./widget-pending-content.element.ts'),
  meta : {
    width:4,
    height:3
  }
}

export const manifests = [
  widget
]
