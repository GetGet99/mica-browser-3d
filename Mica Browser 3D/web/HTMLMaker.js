//@ts-check
class EasyElement {
    /**
     *  @param {{
     *      ElementTag : string,
     *      Apply? : (Element: HTMLElement) => void,
     *      Children? : EasyElement[],
     *  }} obj 
     */
    constructor(obj) {
        /**
         * @type {HTMLElement}
         */
        this.DOMElement = document.createElement(obj.ElementTag)
        if ('Children' in obj) {
            for (const child of obj.Children) {
                this.DOMElement.appendChild(child.DOMElement)
            }
        }
        if ('Apply' in obj) {
            obj.Apply(this.DOMElement)
        }
    }
}
class DIVElement extends EasyElement {
    /**
     *  @param {{
     *      Apply? : (Element: HTMLDivElement) => void,
     *      Children? : EasyElement[],
     *  }} obj 
     */
    constructor(obj) {
        //@ts-ignore
        obj.ElementTag = "div"
        //@ts-ignore
        super(obj)
        /**
         * @type {HTMLDivElement}
         */
        this.DOMElement
    }
}
class IFrameElement extends EasyElement {
    /**
     *  @param {{
     *      Apply? : (Element: HTMLIFrameElement) => void,
     *      Children? : EasyElement[],
     *  }} obj 
     */
    constructor(obj) {
        //@ts-ignore
        obj.ElementTag = "iframe"
        //@ts-ignore
        super(obj)
        /**
         * @type {HTMLIFrameElement}
         */
        this.DOMElement
    }
}
class ButtonElement extends EasyElement {
    /**
     *  @param {{
     *      Apply? : (Element: HTMLButtonElement) => void,
     *      Children? : EasyElement[],
     *  }} obj 
     */
    constructor(obj) {
        //@ts-ignore
        obj.ElementTag = "button"
        //@ts-ignore
        super(obj)
        /**
         * @type {HTMLButtonElement}
         */
        this.DOMElement
    }
}