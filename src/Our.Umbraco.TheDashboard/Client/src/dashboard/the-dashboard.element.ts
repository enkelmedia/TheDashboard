import { LitElement,css,html,customElement, state, repeat, when} from '@umbraco-cms/backoffice/external/lit';;
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
@customElement('the-dashboard-dashboard')
export class TheDashboardDashboardElement extends UmbElementMixin(LitElement) {

  @state()
  recentActivities? : RecentActivitiesFrontendModel;

  @state()
  pendingContent? : PendingContentNotScheduledFrontendModel;

  @state()
  counters? : CountersFrontendModel;

  connectedCallback(): void {
    super.connectedCallback();

    TheDashboardResource.getAllRecentActivities().then((res)=>{
      this.recentActivities = res.data;
    });
    TheDashboardResource.getPending().then((res)=>{
      this.pendingContent = res.data;
    });
    TheDashboardResource.getCounters().then((res)=>{
      this.counters = res.data;
    });

  }

  render() {

    return html`
      <div id="layout">
        <div>
          <the-dashboard-box
            headline=${this.localize.term("theDashboard_recentActivities")}
            description=${this.localize.term('theDashboard_recentActivitiesDescription')}
            >

            ${when(this.recentActivities,()=>html`
              ${repeat(this.recentActivities!.allItems,
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
                        ${when(item.activityType == 'SaveAndScheduled',()=>html`
                          ${item.user.name} ${this.localize.term('theDashboard_savedAndScheduled')} <a href="section/content/workspace/document/edit/${item.nodeKey}">${item.nodeName}</a> ${this.localize.term('theDashboard_forPublishingAt')} ${this.localize.date(item.scheduledPublishDate!,DateTimeOptions)}.
                        `)}
                        ${when(item.activityType == 'Publish',()=>html`
                          ${item.user.name} ${this.localize.term('theDashboard_savedAndPublished')} <a href="section/content/workspace/document/edit/${item.nodeKey}">${item.nodeName}</a>.
                        `)}
                        ${when(item.activityType == 'Unpublish',()=>html`
                          ${item.user.name} ${this.localize.term('theDashboard_unpublished')} <a href="section/content/workspace/document/edit/${item.nodeKey}">${item.nodeName}</a>.
                        `)}
                        ${when(item.activityType == 'RecycleBin',()=>html`
                          ${item.user.name} ${this.localize.term('theDashboard_moved')} <a href="section/content/workspace/document/edit/${item.nodeKey}">${item.nodeName}</a> ${this.localize.term('theDashboard_to')} ${this.localize.term('theDashboard_recycleBin')}.
                        `)}
                        ${when(item.activityType == 'RollBack',()=>html`
                          ${item.user.name} ${this.localize.term('theDashboard_rolledBack')} <a href="section/content/workspace/document/edit/${item.nodeKey}">${item.nodeName}</a>.
                        `)}
                      </p>
                    </div>
                  </div>
              `)}
            `)}
          </the-dashboard-box>
        </div>
        <div>
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
          <the-dashboard-box
            headline=${this.localize.term("theDashboard_yourRecentActivity")}
            description=${this.localize.term('theDashboard_yourRecentActivitiesDescription')}>
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
          </the-dashboard-box>
        </div>
        <div>
          ${when(this.counters,()=>html`
            ${repeat(this.counters!.counters,
              (item)=>item.text,
              (counter)=>{

                const text = counter.localizationKey != '' ? this.localize.term(counter.localizationKey) : counter.text;

                return html`
                  <div class="counter">
                    <div>
                      <span class="dot ${counter.style}">${counter.count}</span>
                    </div>
                    <div>
                      <p>
                        ${when(counter.clickUrl,
                          ()=>html`<a href=${counter.clickUrl}>${text}</a>`,
                          ()=>html`${text}`
                        )}
                      </p>
                    </div>
                  </div>
              `})}
          `)}
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
      display: block;
      margin: 5px 0 0 0;
      line-height: 16px;
      font-weight:normal;
    }

    #layout {
      display:flex;
      gap:20px;
    }
    #layout > div {
      flex-grow: 1;
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

    .counter {
      display:flex;
      gap:10px;
      margin-bottom:10px;
    }

    .dot {
      display:inline-flex;
      justify-content:center;
      align-items:center;
      background-color: cadetblue;
      color: #fff;
      border-radius: 25px;
      width: 50px;
      height: 50px;
    }

    .dot.standard {background:#3544b1;}
    .dot.action {background:var(--uui-color-emphasis, #3544b1);}
    .dot.success {background:var(--uui-color-positive);}
    .dot.warning {background:var(--uui-color-warning);}
    .dot.selected {background:var(--uui-color-current,#f5c1bc);}
    .dot.danger {background:var(--uui-color-danger);}

  `]
}

export default TheDashboardDashboardElement;

declare global {
    interface HTMLElementTagNameMap {
        'the-dashboard-dashboard': TheDashboardDashboardElement;
    }
}
