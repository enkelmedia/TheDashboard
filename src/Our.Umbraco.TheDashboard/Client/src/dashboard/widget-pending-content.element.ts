import { LitElement,css,html,customElement, state, unsafeCSS, when, repeat} from '@umbraco-cms/backoffice/external/lit';;
import { UmbElementMixin } from '@umbraco-cms/backoffice/element-api';
import { UmbTextStyles } from '@umbraco-cms/backoffice/style';
import '@umbraco-cms/backoffice/components';
import { CountersFrontendModel, PendingContentNotScheduledFrontendModel, RecentActivitiesFrontendModel, TheDashboardResource } from '../backend-api';
import './../components/box/the-dashboard-box.element';

const DateTimeOptions: Intl.DateTimeFormatOptions = {
  weekday: 'short',
  year: 'numeric',
  month: 'numeric',
  day: 'numeric',
  hour : '2-digit',
  minute : '2-digit',
  hourCycle : 'h23'
};

/**
* the-dashboard-dashboard description
* @element the-dashboard-dashboard
*/
@customElement('tdd-widget-pending')
export class TheDashboardDashboardElement extends UmbElementMixin(LitElement) {


  @state()
  pendingContent? : PendingContentNotScheduledFrontendModel;


  connectedCallback(): void {
    super.connectedCallback();

    TheDashboardResource.getPending().then((res)=>{
      console.log('pending',res);
      this.pendingContent = res;
    });

  }

  render() {

    return html`
     <the-dashboard-box
            headline=${this.localize.term("theDashboard_pendingContent")}
            description=${this.localize.term('theDashboard_pendingContentDescription')}
            counter=${this.pendingContent?.count ?? 0}
            expandable>
            ${when(this.pendingContent,()=>html`
              ${repeat(this.pendingContent!.items,
                (item)=>item.nodeKey,
                (item)=>html`
                  <div class="activity">
                    <div>
                      <uui-avatar img-src=${item.user.avatar.src} img-srcset=${item.user.avatar.srcSet} name=${item.user.name}></uui-avatar>
                    </div>
                    <div>
                      <span>${this.localize.date(item.datestamp,DateTimeOptions)}</span>
                      <p>
                        ${when(item.activityType == 'Save',()=>html`
                          ${item.user.name} ${this.localize.term('theDashboard_saved')} <a href="section/content/workspace/document/edit/${item.nodeKey}">${item.nodeName}</a> ${this.localize.term('theDashboard_butDidNotPublish')}.
                        `)}
                      </p>
                    </div>
                  </div>
              `)}
            `)}
          </the-dashboard-box>
    `
  }

  static styles = [UmbTextStyles, css`

    * {
      box-sizing:border-box;
    }

    .activity {
      display:flex;
      gap:15px;
      border-bottom:1px solid #f3f3f3;
      margin-bottom:2.5px;
    }
    .activity + .activity {
      padding-top:2.5px;
    }
    .activity span {
      display:block;
      color: #828282;
      font-size:12px;
      font-style:italic;
      line-height:12px;
    }
    .activity p {
      margin:0;
      line-height:1.4em;
    }

  `]
}

export default TheDashboardDashboardElement;

declare global {
    interface HTMLElementTagNameMap {
        'tdd-widget-pending': TheDashboardDashboardElement;
    }
}
