import { LitElement,css,html,customElement,property, when, classMap} from '@umbraco-cms/backoffice/external/lit';;
import { UmbElementMixin } from '@umbraco-cms/backoffice/element-api';
import { UmbTextStyles } from '@umbraco-cms/backoffice/style';

/** ns-box */
@customElement('the-dashboard-box')
export class TheDashboardBoxElement extends UmbElementMixin(LitElement) {

    /**
     * The inputmode global attribute is an enumerated attribute that hints at the type of data that might be entered by the user while editing the element or its contents. This allows a browser to display an appropriate virtual keyboard.
     * @see {@link https://developer.mozilla.org/en-US/docs/Web/HTML/Global_attributes/inputmode|MDN} for further information
     * @type {string}
     * @attr
     */
    @property()
    headline? : string;

    @property()
    description? : string;

    @property({type:Boolean})
    expandable : boolean = false;

    @property({type:Boolean})
    expanded : boolean = false;

    @property({type:String})
    counter? : string;

    render() {
        return html`
          <div id="box" class=${classMap({'expandable':this.expandable, 'expanded' : this.expanded})}>
            <div id="header">
              ${when(this.counter,()=>html`
                <span id="counter">${this.counter}</span>
              `)}
              <div>
                <h2>${this.headline}</h2>
                <small>${this.description}</small>
              </div>
              <div>
                ${when(this.expandable,()=> html`
                  <button @click=${()=> this.expanded = !this.expanded}>
                    <svg width="13" height="12" viewBox="0 0 13 12" fill="none" xmlns="http://www.w3.org/2000/svg">
                        <path d="M6.5 12L0.00480941 0.75L12.9952 0.750001L6.5 12Z" fill="#C4C4C4" />
                    </svg>
                  </button>
                `)}
              </div>
            </div>
            <div id="slot">
              <div>
                <div>
                  <slot></slot>
                </div>
              </div>
            </div>
          </div>
        `

    }

    static styles = [UmbTextStyles, css`
      * {
        box-sizing:border-box;
      }
        :host {
            display:block;
            margin-bottom:15px;
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

        #box {
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
        }

        #header {
          position:relative;
          display:flex;
          justify-content:space-between;
          align-items:center;
          padding: var(--uui-box-header-padding, var(--uui-size-space-4, 12px) var(--uui-size-space-5, 18px));
          border-bottom: 1px solid var(--uui-color-divider-standalone, #e9e9eb);
        }

        #box #slot {
          display:grid;
          grid-template-rows: 1fr;
          transition: grid-template-rows 200ms;
        }

        #box #slot > div > div {
          padding: var(--ns-box-padding, var(--uui-size-5));
        }

        #box.expandable #slot {
          grid-template-rows: 0fr;
        }

        #box.expandable #slot > div {
          overflow:hidden;
        }

        #box.expandable.expanded #slot {
          grid-template-rows: 1fr;
        }

        #header button {
          border: none;
          margin: 0;
          padding: 0;
          width: auto;
          overflow: visible;
          text-align: inherit;
          outline:none;
          cursor:pointer;

          background: transparent;

          color: inherit;
          font: inherit;

          line-height: normal;

          /* Corrects font smoothing for webkit */
          -webkit-font-smoothing: inherit;
          -moz-osx-font-smoothing: inherit;

          -webkit-appearance: none;

        }

        #header button::-moz-focus-inner {
          border: 0;
          padding: 0;
        }

        #box button svg {
          transition: transform .3s;
        }

        #box.expanded button svg {
          transform: rotate(180deg);
        }



        #counter {
          position: absolute;
          left: -15px;
          top: -15px;
          font-size: 12px;
          background: #F5C1BC;
          width: 30px;
          height: 30px;
          display: flex;
          justify-content: center;
          align-items: center;
          border-radius: 200px;
        }
        `
    ]
}
declare global {
    interface HTMLElementTagNameMap {
        'the-dashboard-box': TheDashboardBoxElement;
    }
}
