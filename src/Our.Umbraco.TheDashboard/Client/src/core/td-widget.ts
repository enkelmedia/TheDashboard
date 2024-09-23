import { UmbElement } from "@umbraco-cms/backoffice/element-api";
import { ManifestElement } from "@umbraco-cms/backoffice/extension-api";
import internal from "stream";

export interface ManifestTdWidget extends ManifestElement<UmbElement> {
  type: 'tdWidget';
  meta : TdWidgetMeta;
}

export interface TdWidgetMeta {
  width : number;
  height : number;
  //entityType : string;
  //label : string;
}
