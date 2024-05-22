import { LitElement,css,html,customElement,property, when} from '@umbraco-cms/backoffice/external/lit';;
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

    @property({ attribute: 'cta-text' })
    ctaText? : string;

    @property({ attribute: 'cta-href' })
    ctaHref? : string;

    @property({type:Boolean})
    expandable : boolean = false;

    @property({type:Boolean})
    expanded : boolean = false;


    render() {
        return html`
            ${when(this.headline,()=>html`<h2>${this.headline}</h2>`)}
            <div id="box">
                <div id="slot">
                    <slot></slot>
                </div>
                ${when(this.ctaText,()=>html`
                    <div id="cta">
                        <a href="${this.ctaHref!}">${this.ctaText}</a>
                    </div>
                `)}
            </div>
        `

    }

    static styles = [UmbTextStyles, css`
        :host {
            display:block;
        }

        h2 {
            font-size:15px;
            font-weight:bold;
            margin: var(--uui-size-2) 0;
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

        #slot {
            padding: var(--ns-box-padding, var(--uui-size-5));
        }

        #cta {
            border-top:1px solid var(--uui-color-border, #d8d7d9);;
            padding: var(--ns-box-padding, var(--uui-size-5));
            text-align:center;
        }

        `
    ]
}
declare global {
    interface HTMLElementTagNameMap {
        'the-dashboard-box': TheDashboardBoxElement;
    }
}
