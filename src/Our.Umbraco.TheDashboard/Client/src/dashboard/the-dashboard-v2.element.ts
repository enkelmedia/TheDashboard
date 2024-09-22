import { LitElement,css,html,customElement, state, unsafeCSS} from '@umbraco-cms/backoffice/external/lit';;
import { UmbElementMixin } from '@umbraco-cms/backoffice/element-api';
import { UmbTextStyles } from '@umbraco-cms/backoffice/style';
import '@umbraco-cms/backoffice/components';
import { CountersFrontendModel, PendingContentNotScheduledFrontendModel, RecentActivitiesFrontendModel } from '../backend-api';
import './../components/box/the-dashboard-box.element';
import { GridStack, GridStackElement } from 'gridstack';
import style from 'gridstack/dist/gridstack.min.css?inline';
import './widget.element'
import './widget-my-activities.element'
import './widget-counters.element'
import './widget-pending-content.element'

/**
* the-dashboard-dashboard description
* @element the-dashboard-dashboard
*/
@customElement('the-dashboard-dashboard')
export class TheDashboardDashboardElement extends UmbElementMixin(LitElement) {

  @state()
  recentActivities? : RecentActivitiesFrontendModel;

  @state()
  pendingContent? : PendingContentNotScheduledFrontendModel;

  @state()
  counters? : CountersFrontendModel;

  _grid? : GridStack;

  firstUpdated(): void {
    //super.connectedCallback();

    const elm = this.shadowRoot?.querySelector('.grid-stack') as GridStackElement;

    console.log('elm',elm);

    var items = [
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

    ];
    this._grid = GridStack.init({
      //minRow:2,
      cellHeight:50
    },elm);
    this._grid.load(items);
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
