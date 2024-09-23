import { LitElement,css,html,customElement, unsafeCSS} from '@umbraco-cms/backoffice/external/lit';
import { UmbElementMixin } from '@umbraco-cms/backoffice/element-api';
import { UmbTextStyles } from '@umbraco-cms/backoffice/style';
import '@umbraco-cms/backoffice/components';
import './../components/box/the-dashboard-box.element';
import { GridStack, GridStackElement, GridStackWidget } from 'gridstack';
import style from 'gridstack/dist/gridstack.min.css?inline';
import { ManifestTdWidget } from '../core/td-widget';
import { umbExtensionsRegistry } from '@umbraco-cms/backoffice/extension-registry';
import { createExtensionElement, loadManifestElement, UmbExtensionsManifestInitializer } from '@umbraco-cms/backoffice/extension-api';
import { UmbLitElement } from '@umbraco-cms/backoffice/lit-element';

/**
* the-dashboard-dashboard description
* @element the-dashboard-dashboard
*/
@customElement('the-dashboard-dashboard')
export class TheDashboardDashboardElement extends UmbElementMixin(LitElement) {

  _grid? : GridStack;
  _elements : UmbLitElement[] = [];
  _widgets : ManifestTdWidget[] = [];

  connectedCallback() : void {
    super.connectedCallback();

    new UmbExtensionsManifestInitializer(
      this,
      umbExtensionsRegistry,
      'tdWidget',
      (f : ManifestTdWidget) => f != null,
      async (viewManifests) => {

        if(viewManifests.length == 0){
          //this._emailProviderSettingsElement = undefined;
          return;
        }

        console.log('widgets',viewManifests);

        this._widgets = viewManifests.map(x=>x.manifest);



        this.initGrid();


        //this._emailProviderSettingsElement = await createExtensionElement(viewManifests[0].manifest) as any as NsEmailServiceProviderUiBase<any>;
        //this._emailProviderSettingsElement.settings = this.model.emailServiceProviderValue.settings;
        //this._emailProviderSettingsElement.overriddenSettings = this.serverData.overriddenEmailServiceProviderSettings;
        //this._emailProviderSettingsElement.addEventListener('change', (e : Event) => {
          //let evt = e as NsSmtpProviderSettingsChangedEvent;
         // this.model.emailServiceProviderValue.settings = evt.detail;
       // });

        //this.requestUpdate();



    },
    "theDashboardLoadWidgets");

  }

  firstUpdated(): void{

  }

  initGrid(): void {
    //super.connectedCallback();

    const elm = this.shadowRoot?.querySelector('.grid-stack') as GridStackElement;

    console.log('elm',elm);

    var items : GridStackWidget[] = [];
      /*
      {
        w:4,
        h:10,
        content: `<div><h2>${this.localize.term("theDashboard_recentActivities")}</h2><small>${this.localize.term('theDashboard_recentActivitiesDescription')}</small></div><tdd-widget-activities></tdd-widget-activities>`
      },
      {
        w:4,
        h:3,
        content: `<tdd-widget-pending></tdd-widget-pending>`
      },

      {
        w:4,
        h:10,
        content: `<tdd-widget-counters></tdd-widget-counters>`
      },
      {
        w:4,
        h:7,
        content: `<div><h2>${this.localize.term("theDashboard_yourRecentActivity")}</h2><small>${this.localize.term('theDashboard_yourRecentActivitiesDescription')}</small></div><tdd-widget-my-activities></tdd-widget-my-activities>`
      },
      */
    //];

    console.log('elements', this._elements.length);
/*
    this._elements.forEach((instance)=>{
      items.push({
        w: 4,
        h:7,
        content : `<${instance.nodeName}></${instance.nodeName}>`
      });
    });
*/
    this._grid = GridStack.init({
      //minRow:2,
      cellHeight:50
    },elm);
    //this._grid.load(items);

    /*
    await Promise.all(viewManifests.map(async (manifest) => {


      let instance = await createExtensionElement(manifest.manifest) as any as UmbLitElement;

      console.log('add', instance.nodeName);
      this._elements.push(instance);

    }));
    */

    console.log('lets init grid');

    this._widgets.forEach(async (manifest)=>{
      this.addGridStackElement(manifest);
    });


  }

  async addGridStackElement(manifest : ManifestTdWidget){

    var instance = await createExtensionElement(manifest) as any as UmbLitElement;

    // <div class="grid-stack-item ${this.opts.itemClass || ''}"><div class="grid-stack-item-content">${content}</div></div>
    let itemElm = document.createElement('div');
    itemElm.classList.add("grid-stack-item");

    let contentItemElm = document.createElement("div");
    contentItemElm.classList.add("grid-stack-item-content");

    contentItemElm.appendChild(instance);
    itemElm.appendChild(contentItemElm);

    this._grid!.addWidget(itemElm,{
      w : manifest.meta.width,
      h : manifest.meta.height,
      minW:4,
      minH:4,
      maxW:4,
      maxH:10,
    });

  }

  handleSave(){
    console.log(this._grid!.getGridItems());
  }

  render() {


    return html`
      <div id="layout">
        <div class="toolbar">
          <button @click=${this.handleSave}>Save</button>
        </div>
        <div class="grid-stack"></div>
      </div>
  `
  }

  static styles = [UmbTextStyles,unsafeCSS(style), css`

    * {
      box-sizing:border-box;
    }

    .toolbar {
      display:flex;
      justify-content:flex-end;
    }

    /*.grid-stack { background: #ffffff; }*/
    .grid-stack-item-content {
      background:var(--uui-color-surface);

      /*
      Old ns-box style
      border-radius: 3px;
      border: 1px solid #EBEBEB;
      box-shadow: 0 1px 1px 0 rgba(0,0,0,0.16);
      */

      box-shadow: var(--uui-shadow-depth-1, 0 1px 3px rgba(0, 0, 0, 0.12), 0 1px 2px rgba(0, 0, 0, 0.24));
      border-radius: var(--uui-border-radius, 3px);
      background-color: var(--uui-color-surface, #fff);

      padding:10px;
    }

    h2 {
          font-size:15px;
          font-weight:bold;
          margin:0;
          line-height:15px;
      }
      small {
        display:block;
        color: #828282;
        margin: 5px 0 0 0;
        line-height: 1.4em;
        font-weight:normal;
      }



  `]
}

export default TheDashboardDashboardElement;

declare global {
    interface HTMLElementTagNameMap {
        'the-dashboard-dashboard': TheDashboardDashboardElement;
    }
}
