import { LitElement,css,html,customElement, state, when, repeat} from '@umbraco-cms/backoffice/external/lit';;
import { UmbElementMixin } from '@umbraco-cms/backoffice/element-api';
import { UmbTextStyles } from '@umbraco-cms/backoffice/style';
import '@umbraco-cms/backoffice/components';
import { RecentActivitiesFrontendModel, TheDashboardResource } from '../../backend-api';
import '../../components/box/the-dashboard-box.element';

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
@customElement('tdd-widget-my-activities')
export class TheDashboardDashboardElement extends UmbElementMixin(LitElement) {

  @state()
  recentActivities? : RecentActivitiesFrontendModel;

  connectedCallback(): void {
    super.connectedCallback();

    TheDashboardResource.getAllRecentActivities().then((res)=>{
      console.log('recent',res);
      this.recentActivities = res;
    });
  }

  render() {

    return html`


      ${when(this.recentActivities,()=>html`
        ${repeat(this.recentActivities!.yourItems,
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
                    ${this.localize.term('theDashboard_Saved')} <a href="section/content/workspace/document/edit/${item.nodeKey}">${item.nodeName}</a> ${this.localize.term('theDashboard_butDidNotPublish')}.
                  `)}
                  ${when(item.activityType == 'SaveAndScheduled',()=>html`
                    ${this.localize.term('theDashboard_SavedAndScheduled')} <a href="section/content/workspace/document/edit/${item.nodeKey}">${item.nodeName}</a> ${this.localize.term('theDashboard_forPublishingAt')} ${this.localize.date(item.scheduledPublishDate!,DateTimeOptions)}.
                  `)}
                  ${when(item.activityType == 'Publish',()=>html`
                    ${this.localize.term('theDashboard_SavedAndPublished')} <a href="section/content/workspace/document/edit/${item.nodeKey}">${item.nodeName}</a>.
                  `)}
                  ${when(item.activityType == 'Unpublish',()=>html`
                    ${this.localize.term('theDashboard_Unpublished')} <a href="section/content/workspace/document/edit/${item.nodeKey}">${item.nodeName}</a>.
                  `)}
                  ${when(item.activityType == 'RecycleBin',()=>html`
                    ${this.localize.term('theDashboard_Moved')} <a href="section/content/workspace/document/edit/${item.nodeKey}">${item.nodeName}</a> ${this.localize.term('theDashboard_to')} ${this.localize.term('theDashboard_recycleBin')}.
                  `)}
                  ${when(item.activityType == 'RollBack',()=>html`
                    ${this.localize.term('theDashboard_RolledBack')} <a href="section/content/workspace/document/edit/${item.nodeKey}">${item.nodeName}</a>.
                  `)}
                </p>
              </div>
            </div>
        `)}
      `)}

  `
  }

  static styles = [UmbTextStyles, css`

    * {
      box-sizing:border-box;
    }

    :host {
      display:block;
      margin-top:10px;
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
        'tdd-widget-my-activities': TheDashboardDashboardElement;
    }
}
