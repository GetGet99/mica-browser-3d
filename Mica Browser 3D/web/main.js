//@ts-check

const scence = new THREE.Scene();
const SimulatedScence = new THREE.Scene();
const CenterSimulator = new THREE.Group();
const OrbidSimulator = new THREE.Group();
const OrbidObjectSimulator = new THREE.Group();
const camera = new THREE.PerspectiveCamera(90, window.innerWidth / window.innerHeight, 0.1, 1000);
const focustaker = document.getElementById('focustaker')
const renderer = new CSS3DRenderer();
renderer.setSize(window.innerWidth, window.innerHeight);
window.onresize = function () {
    camera.aspect = window.innerWidth / window.innerHeight;
    camera.updateProjectionMatrix();
    renderer.setSize(window.innerWidth, window.innerHeight);
}
document.body.appendChild(renderer.domElement);
/**
 * @param {Event} ev
 */
function BlockEventIfCursorLock(ev) {
    if (cursorLock) {
        ev.preventDefault()
        ev.stopPropagation()
    }
}
/**
 * @param {string} src
 * @param {number} x
 * @param {number} y
 * @param {number} z
 * @param {number} ry
 */
function IFrame( src, x, y, z, ry ) {

    // const div = document.createElement( 'div' );
    // div.style.width = '1600px';
    // div.style.height = '900px';
    // div.style.backgroundColor = '#000';
    const iframe = new IFrameElement({
        Apply: function (iframe) {
            iframe.style.width = '100%';
            iframe.style.height = 'calc(100% - 40px)';
            iframe.style.borderRadius = '10px';
            iframe.style.border = '0px';
            iframe.src = src;
        }
    })
    let ele = new DIVElement({
        Apply: function (div) {
            div.style.width = '1600px';
            div.style.height = '1000px';
        },
        Children: [
            new DIVElement({
                Apply: function (div) {
                    div.style.height = '40px';
                    div.style.paddingBottom = '10px';
                },
                Children: [
                    new DIVElement({
                        Apply: function (div) {
                            div.style.background = 'hsla(0 0% 100% / .03)';
                            div.style.borderRadius = '10px';
                            div.style.height = '100%';
                            div.style.width = '100%';
                            div.style.display = 'flex';
                            div.style.alignItems = 'center';
                        },
                        Children: [
                            new DIVElement({
                                Apply: function(dragable) {
                                    dragable.style.flexGrow = '1';
                                    dragable.style.fontFamily = '';
                                    dragable.style.height = '100%';
                                    let translationVector = new THREE.Vector3()
                                    
                                    // const TranslateXAxis = new THREE.Vector3( 0, 1, 0 );
                                    // const TranslateYAxis = new THREE.Vector3( 0, 0, 1 );
                                    // const TranslateCloserAxis = new THREE.Vector3( 1, 0, 0 );
                                    let distance = 0
                                    let ismousedown = false
                                    dragable.onmousedown = function (ev) {
                                        const cameraquaternion = camera.getWorldQuaternion(new THREE.Quaternion())
                                        const cameradirection = camera.getWorldDirection(new THREE.Vector3())
                                        const cameraposition = camera.getWorldPosition(new THREE.Vector3())
                                        SimulatedScence.attach(CenterSimulator)
                                        CenterSimulator.position.copy(cameraposition)
                                        CenterSimulator.quaternion.copy(cameraquaternion)
                                        const ray = new THREE.Ray()
                                        // const TranslateRightVector = object.getWorldDirection(new THREE.Vector3());
                                        // const TranslateDownVector = object.getWorldDirection(new THREE.Vector3());
                                        // const TranslateCloserVector = object.getWorldDirection(new THREE.Vector3());
                                        // const degree90 = Math.PI / 2; // 90 degree

                                        // TranslateRightVector.applyAxisAngle(TranslateXAxis, degree90);
                                        // TranslateCloserVector.applyAxisAngle(TranslateYAxis, degree90);
                                        // TranslateDownVector.applyAxisAngle(TranslateCloserAxis, degree90);
                                        // const rect = ele.getBoundingClientRect()
                                        // const distanceXfromhalf = ev.clientX
                                        // const distanceYfromhalf = ev.clientY
                                        // translationVector.set(0, 0, 0)
                                        // TranslateRightVector.multiplyScalar(distanceXfromhalf)
                                        // TranslateDownVector.multiplyScalar(distanceYfromhalf)
                                        // translationVector.add(TranslateRightVector).add(TranslateDownVector)
                                        // const Point = object.position.clone()
                                        // Point.add(translationVector)

                                        // distance = camera.getWorldPosition(new THREE.Vector3()).distanceTo(Point)
                                        // TranslateCloserVector.multiplyScalar(10)
                                        // const newvec = object.position.add(TranslateDownVector)
                                        
                                        // const vector = new THREE.Vector3( ( ev.clientX / window.innerWidth ) * 2 - 1, 0.5, - ( ev.clientY / window.innerHeight ) * 2 + 1 );
                                        // vector.sub(cameraposition).normalize();
                                        ray.direction.copy(cameradirection);
                                        ray.origin.copy(cameraposition);
                                        const intersect = ray.intersectPlane(new THREE.Plane().setFromNormalAndCoplanarPoint(new THREE.Vector3(0,0,1).applyQuaternion(object.quaternion), object.position), new THREE.Vector3())
                                        if (intersect != null) {
                                            SimulatedScence.attach(OrbidSimulator)
                                            OrbidSimulator.position.copy(intersect)
                                            OrbidSimulator.quaternion.copy(object.quaternion)
                                            CenterSimulator.attach(OrbidSimulator)
                                            SimulatedScence.attach(OrbidObjectSimulator)
                                            OrbidObjectSimulator.position.copy(object.position)
                                            OrbidObjectSimulator.quaternion.copy(object.quaternion)
                                            OrbidSimulator.attach(OrbidObjectSimulator)
                                            // distance = intersect.distanceTo(cameraposition)
                                            // translationVector = ray.origin.clone().add(ray.direction.clone().multiplyScalar(distance)).sub(object.position)
                                            let intervalId = setInterval(function () {
                                                if (!leftbtndown)
                                                    clearInterval(intervalId)
                                                const Position = camera.getWorldPosition(new THREE.Vector3())
                                                const Quaternion = camera.getWorldQuaternion(new THREE.Quaternion())
                                                CenterSimulator.position.copy(Position)
                                                CenterSimulator.quaternion.copy(Quaternion)
                                                object.position.copy(OrbidObjectSimulator.getWorldPosition(new THREE.Vector3()))
                                                const euler = new THREE.Euler().setFromQuaternion(OrbidObjectSimulator.getWorldQuaternion(new THREE.Quaternion()))
                                                if (keyPressing.includes('alt')) {
                                                    // const possibleangles = [-2 * Math.PI,, 2 * Math.PI, -3/4 * Math.PI, 3/4 * Math.PI, -1/2 * Math.PI, 1/2 * Math.PI, -Math.PI, Math.PI, -Math.PI]
                                                    // if (euler.x < Math.PI)
                                                } else
                                                    euler.x = object.rotation.x
                                                euler.z = object.rotation.z
                                                object.rotation.copy(euler)
                                            }, 20)
                                            
                                        }
                                        
                                    }
                                    dragable.onwheel = function (ev) {
                                        let direction;
                                        if (ev.ctrlKey) {
                                            direction = camera.getWorldDirection(new THREE.Vector3())
                                            direction.multiplyScalar(-1)
                                        }
                                        else
                                            direction = object.getWorldDirection(new THREE.Vector3())
                                        if (!ev.shiftKey) {
                                            direction.y = 0
                                            direction.normalize()
                                        }
                                        direction.multiplyScalar(ev.deltaY)
                                        if (ev.deltaMode == WheelEvent.DOM_DELTA_LINE)
                                            direction.multiplyScalar(5)
                                        object.position.add(direction)
                                    }
                                }
                            }),
                            new ButtonElement({
                                Apply: function (btn) {
                                    btn.innerText = "N";
                                    btn.className = 'control-button';
                                    btn.onclick = function () {
                                        iframe.DOMElement.src = window.prompt('Enter the new URL here');
                                    };
                                }
                            }),
                            new ButtonElement({
                                Apply: function (btn) {
                                    btn.innerText = "X";
                                    btn.className = 'control-button';
                                    btn.onclick = () => scence.remove(object);
                                }
                            })
                        ]
                    })
                ]
            }),
            iframe
        ]
    }).DOMElement

    const object = new CSS3DObject( ele );
    object.position.set( x, y, z );
    object.rotation.y = ry;

    return object;

}
// const obj = IFrame(`https://google.com/`, 0, 0, 500, 180)
let cursorLock = false;
// scence.add(obj);
/**
 * @param {string} src
 */
function CreateIFrameInFront(src) {
    const cwd = new THREE.Vector3();
    camera.getWorldDirection(cwd)

    cwd.multiplyScalar(500);
    cwd.add(camera.getWorldPosition(new THREE.Vector3()));
    const frame = IFrame(src, 0,0,0,0)
    frame.position.set(cwd.x, cwd.y, cwd.z);
    const euler = new THREE.Euler()
    euler.setFromQuaternion(camera.getWorldQuaternion(new THREE.Quaternion()))
    frame.rotation.set(euler.x, euler.y, euler.z);
    scence.add(frame);
}
CreateIFrameInFront("https://google.com")

const MouseSensitiveX = 0.1, MouseSensitiveY = 0.1;
camera.position.z = 3;
// update 
function update() {

}
let left = false, forward = false, right = false, back = false;
let leftbtndown = false, rightbtndown = false;
document.onkeydown = BlockEventIfCursorLock;
/**
 * @type {string[]}
 */
let keyPressing = [];
/**
 * @param {{data: {
 *      keys : string[],
 *      cursorLock : boolean,
 *      mouse : {
 *          dX : Number,
 *          dY : Number,
 *          LeftDown : boolean,
 *          RightDown : boolean
 *      }
 * }}} e
 */
function MessageRecieved(e) {
    const data = e.data
    keyPressing = data.keys
    cursorLock = data.cursorLock
    if(cursorLock) {
        focustaker.focus({
            
        })
    }
    leftbtndown = data.mouse.LeftDown
    rightbtndown = data.mouse.RightDown
    let rotX = rotationMouseXDegree + data.mouse.dX * MouseSensitiveX
    if (rotX > 360) rotX -= 360
    if (rotX < 0) rotX = 360 - rotX
    rotationMouseXDegree = rotX
    let rotY = rotationMouseYDegree - (data.mouse.dY * MouseSensitiveY)
    if (rotY > 90) rotY = 90
    if (rotY < -90) rotY = -90
    rotationMouseYDegree = rotY
}
//@ts-ignore
window.chrome.webview.addEventListener('message', MessageRecieved)

let speed = 3, maxSpeed = 3, friction = 0.91, 
    position = { x: 0, y: 0, z: 0 },
    velocity = { x: 0, y: 0, z: 0 },
    rotationMouseXDegree = 0, rotationMouseYDegree = 0;
const cameraPitchObject = new THREE.Object3D();
cameraPitchObject.add( camera );

const cameraYawObject = new THREE.Object3D();
cameraYawObject.position.y = 10;
cameraYawObject.add( cameraPitchObject );
  
function fixedUpdate() {
    if (keyPressing.includes('w') && velocity.z > -maxSpeed) velocity.z -= speed;
    if (keyPressing.includes('s') && velocity.z < maxSpeed) velocity.z += speed;
    if (keyPressing.includes('a') && velocity.x > -maxSpeed) velocity.x -= speed;
    if (keyPressing.includes('d') && velocity.x < maxSpeed) velocity.x += speed;
    if (keyPressing.includes('shift') && velocity.y > -maxSpeed) velocity.y -= speed;
    if (keyPressing.includes('space') && velocity.y < maxSpeed) velocity.y += speed;
    velocity.z *= friction;
    velocity.x *= friction;
    velocity.y *= friction;
    const rotationMouseXRadian = rotationMouseXDegree * Math.PI * 2 / 360;
    position.z += velocity.z * Math.cos(rotationMouseXRadian);
    position.x += velocity.z * Math.sin(rotationMouseXRadian); 
    position.z -= velocity.x * Math.sin(rotationMouseXRadian); 
    position.x += velocity.x * Math.cos(rotationMouseXRadian);
    position.y += velocity.y;
    cameraYawObject.rotation.y = THREE.MathUtils.degToRad(rotationMouseXDegree);
    cameraPitchObject.rotation.x = -THREE.MathUtils.degToRad(rotationMouseYDegree);
    camera.getWorldDirection(new THREE.Vector3());
    // camera.rotation.y = rotationMouseXDegree * Math.PI * 2 / 360;
    // camera.rotation.x = rotationMouseYDegree * Math.PI * 2 / 360;
    cameraYawObject.position.x = position.x;
    cameraYawObject.position.y = position.y;
    cameraYawObject.position.z = position.z;
}
setInterval(fixedUpdate, 20)
addEventListener('resize', e => renderer.setSize(innerWidth, innerHeight))  

function render() {
    renderer.render(scence, camera);
}

function GameLoop() {
    requestAnimationFrame(GameLoop);

    update();
    render();
}
GameLoop();