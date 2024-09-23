import { ManifestTdWidget } from '../../core/td-widget.js'

const widget : ManifestTdWidget = {
	type: "tdWidget",
	alias: "Td.Widget.RecentActivities",
	name: "The Dashboard Widget Recent Activities",
  weight: 100,
  element : ()=>import('./widget.element.ts'),
  meta : {
    width:4,
    height:10
  }
}

export const manifests = [
  widget
]
