import { LitElement,css,html,customElement} from '@umbraco-cms/backoffice/external/lit';;
import { UmbElementMixin } from '@umbraco-cms/backoffice/element-api';
import { UmbTextStyles } from '@umbraco-cms/backoffice/style';
import '@umbraco-cms/backoffice/components';
import { TheDashboardResource } from '../backend-api';

// const DateTimeOptions: Intl.DateTimeFormatOptions = {
//   //weekday: '',
//   year: 'numeric',
//   month: 'numeric',
//   day: 'numeric',
//   hour : '2-digit',
//   minute : '2-digit',
//   hourCycle : 'h24'
// };

/**
* the-dashboard-dashboard description
* @element the-dashboard-dashboard
*/
@customElement('the-dashboard-dashboard')
export class TheDashboardDashboardElement extends UmbElementMixin(LitElement) {

  connectedCallback(): void {
    super.connectedCallback();

    TheDashboardResource.getAllRecentActivities().then((res)=>{
      console.log('recent',res);
    });
    TheDashboardResource.getPending().then((res)=>{
      console.log('pending',res);
    });
    TheDashboardResource.getCounters().then((res)=>{
      console.log('counter',res);
    });

  }

  render() {


    return html`
      <div>
        <uui-box headline=${this.localize.term('theDashboard_recentActivities')}>

        </uui-box>
      </div>

  `
  }

  static styles = [UmbTextStyles, css`

    * {
      box-sizing:border-box;
    }
    :host > div {
      padding:20px;
      display:flex;
      gap:20px;
    }

  `]
}

export default TheDashboardDashboardElement;

declare global {
    interface HTMLElementTagNameMap {
        'the-dashboard-dashboard': TheDashboardDashboardElement;
    }
}
