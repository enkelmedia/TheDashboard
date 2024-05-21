import { LitElement,css,html,customElement, state, repeat, when} from '@umbraco-cms/backoffice/external/lit';;
import { UmbElementMixin } from '@umbraco-cms/backoffice/element-api';
import { UmbTextStyles } from '@umbraco-cms/backoffice/style';
import '@umbraco-cms/backoffice/components';
import { RecentActivitiesFrontendModel, TheDashboardResource } from '../backend-api';

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

  @state()
  recentActivities? : RecentActivitiesFrontendModel;

  connectedCallback(): void {
    super.connectedCallback();

    TheDashboardResource.getAllRecentActivities().then((res)=>{
      console.log('recent',res);
      this.recentActivities = res;
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
      <div id="layout">
        <div>
          <uui-box>
            <div slot="headline" class="uui-text">
              <h5 class="uui-h5">${this.localize.term("theDashboard_recentActivities")}</h5>
              <small>${this.localize.term('theDashboard_recentActivitiesDescription')}</small>
            </div>
            ${when(this.recentActivities,()=>html`
              ${repeat(this.recentActivities!.allItems,
                (item)=>item.nodeId,
                (activity)=>html`
                  <div class="">
                    <uui-avatar img-src=${activity.user.avatar.src} img-srcset=${activity.user.avatar.srcSet} name=${activity.user.name}></uui-avatar>
                    Name: ${activity.nodeName}
                  </div>
              `)}
            `)}
          </uui-box>
        </div>
        <div>
          <uui-box>
            <div slot="headline" class="uui-text">
              <h5 class="uui-h5">${this.localize.term("theDashboard_recentActivities")}</h5>
              <small>${this.localize.term('theDashboard_recentActivitiesDescription')}</small>
            </div>
            <div>

            </div>
          </uui-box>
          <uui-box>
            <div slot="headline" class="uui-text">
              <h5 class="uui-h5">${this.localize.term("theDashboard_recentActivities")}</h5>
              <small>${this.localize.term('theDashboard_recentActivitiesDescription')}</small>
            </div>
            <div>

            </div>
          </uui-box>
        </div>
        <div>
          counters
        </div>
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

    small {
      margin:0;
      font-weight:normal;
    }

    #layout {
      display:flex;
      gap:20px;
    }
    #layout > div {
      flex-grow: 1;
    }

  `]
}

export default TheDashboardDashboardElement;

declare global {
    interface HTMLElementTagNameMap {
        'the-dashboard-dashboard': TheDashboardDashboardElement;
    }
}
