class Drawer {
  #canvas;
  #ctx;
  #x;

  constructor(x) {
    this.#x = x;
    console.info("XXX",x);
  }

  draw(data) {
    console.info("Draw")
  }
}

const d = new Drawer(123);

onmessage = function(e) {
  console.log('Worker: Message received from main script', e.data);
  const result = e.data;
  if (!result) {
    postMessage('Please write two numbers');
  } else {
    console.log('Worker: Posting message back to main script:', result);
    d.draw(e.data.data);

    postMessage(result);
  }
}