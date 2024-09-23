import { LitElement,css,html,customElement, state, when, repeat} from '@umbraco-cms/backoffice/external/lit';
import { UmbElementMixin } from '@umbraco-cms/backoffice/element-api';
import { UmbTextStyles } from '@umbraco-cms/backoffice/style';
import '@umbraco-cms/backoffice/components';
import { CountersFrontendModel, TheDashboardResource } from '../../backend-api';
import '../../components/box/the-dashboard-box.element';

/**
* the-dashboard-dashboard description
* @element the-dashboard-dashboard
*/
@customElement('tdd-widget-counters')
export class TheDashboardDashboardElement extends UmbElementMixin(LitElement) {

  @state()
  counters? : CountersFrontendModel;

  connectedCallback(): void {
    super.connectedCallback();

    TheDashboardResource.getCounters().then((res)=>{
      console.log('counter',res);
      this.counters = res;
    });

  }

  render() {

    return html`
      ${when(this.counters,()=>html`
            ${repeat(this.counters!.counters,
              (item)=>item.text,
              (counter)=>html`
                <div class="counter">
                  <div>
                    <span class="dot ${counter.style}">${counter.count}</span>
                  </div>
                  <div>
                    <p>
                      ${when(counter.localizationKey,()=>html`${this.localize.term(counter.localizationKey)}`)}
                      ${when(!counter.localizationKey,()=>html`${counter.text}`)}
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
        'tdd-widget-counters': TheDashboardDashboardElement;
    }
}
