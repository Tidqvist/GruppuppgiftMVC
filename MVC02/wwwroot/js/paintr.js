/**
 * Color object definition. Used when randomizing drawing colors or to pre-define them.
 * @param {Number} r
 * @param {Number} g
 * @param {Number} b
 * @param {Number} transparency
 */



function Color(r, g, b, transparency) {
  var self = this;

  self.r = r;
  self.g = g;
  self.b = b;
  if (typeof(transparency) === "undefined") {
    self.transparency = 1;
  } else {
    self.transparency = transparency;
  }

  self.rgb = r + "," + g + "," + b;
}
/**
 * Return the Color object rgb code, in the format "(xxx,yyy,zzz)"
 * @return {String} rgb
 */
Color.prototype.returnRGBCode = function() {
  var self = this;
  console.log(self.rgb);
  return self.rgb;
};

//GENERATION OPTIONS
var base_seed = 2, //Size of the left half of the grid, used to calculate entire size
  base_height = (base_seed * 2) + 1, //Final size of the grid
  //FIXME Brush size leaves empty white lines between the pixels
  brush_size = 100 / base_height, //Brush size
  drawBg = true; //Not functional for now, keep true


/**
 * Randomizes the first half of the grid; used in simmetric generation
 * @param {Array} grid
 * @return {Array} map
 */

function randomizeHalfGrid(grid) {
  var map = grid;
  for (var i = 0; i < base_seed; i++) {
    map[i] = [];
    for (var j = 0; j < base_height; j++) {
      map[i][j] = Math.floor(Math.random() * 2);
    }
  }
  return map;
}

/**
 * Randomizes the center of the grid; used in simmetric generation
 * @return {Array} center
 */

function randomizeCenterGrid() {
  var center = [];
  for (var i = 0; i < base_height; i++) {
    center.push(Math.floor(Math.random() * 2));
  }
  return center;
}

var red = new Color(200, 0, 0);
var green = new Color(0, 200, 0);
var blue = new Color(0, 0, 200);

/**
 * Randomizes a color for drawing; creates a new Color object and assigns a RGB value to its properties.
 * If a color is passed as argument, then that color will be returned.
 * @param {Color} color (optional)
 * @return {Color} color
 */

function randomizeRGB(color) {
  if (typeof(color) === "undefined") {
    color = new Color();
    color.r = Math.floor((Math.random() * 200) + 0);
    color.g = Math.floor((Math.random() * 200) + 0);
    color.b = Math.floor((Math.random() * 200) + 0);
  }
  return color;
}

/**
 * Main function that handles the drawing in the canvas.
 */

function drawAll() {

  var grid = [];

  grid = randomizeHalfGrid(grid);
  var center = randomizeCenterGrid();

  var canvas = document.getElementById('canvas');
  canvas.style.display = "block";
  avatarDiv = document.getElementById('avatar');
  avatarDiv.style.display = "none";

  if (canvas.getContext) {
  console.log("i draw all")

    var drw = canvas.getContext("2d");
    console.log(drw);

    var color = randomizeRGB(),
      bg_color = randomizeRGB();

    var coordx = 0,
      coordy = 0;

    for (var i = 0; i < base_seed; i++) {
      for (var j = 0; j < base_height; j++) {
        //console.log("Drawing normal " + grid[i][j] + " at " + coordx + "," +coordy);
        if (grid[i][j]) {
          drw.fillStyle = "rgb(" + color.r + "," + color.g + "," + color.b + ")";
        } else if (drawBg && !grid[i][j]) {
          drw.fillStyle = "rgb(" + bg_color.r + "," + bg_color.g + "," + bg_color.b + ")";
        }
        drw.fillRect(coordx, coordy, brush_size, brush_size);
        coordy += brush_size;
      }
      coordx += brush_size;
      coordy = 0;
    }

    for (var x = 0; x < base_height; x++) {
      //console.log("Drawing center " + center[x] + " at " + coordx + "," +coordy);
      if (center[x]) {
        drw.fillStyle = "rgb(" + color.r + "," + color.g + "," + color.b + ")";
      } else if (drawBg) {
        drw.fillStyle = "rgb(" + bg_color.r + "," + bg_color.g + "," + bg_color.b + ")";
      }
      drw.fillRect(coordx, coordy, brush_size, brush_size);
      coordy += brush_size;
    }

    coordx += brush_size;
    coordy = 0;
    //Print the grid in reverse column order
    var ireverse = base_seed - 1;
    for (var k = 0; k < base_seed; k++) {
      for (var z = 0; z < base_height; z++) {
        //console.log("Drawing reverse " + grid[ireverse][z] + " at " + coordx + "," +coordy);
        if (grid[ireverse][z]) {
          drw.fillStyle = "rgb(" + color.r + "," + color.g + "," + color.b + ")";
        } else if (drawBg) {
          drw.fillStyle = "rgb(" + bg_color.r + "," + bg_color.g + "," + bg_color.b + ")";
        }
        drw.fillRect(coordx, coordy, brush_size, brush_size);
        coordy += brush_size;
      }
      ireverse -= 1;
      coordx += brush_size;
      coordy = 0;
    }
  }
}

$("#btnSave").click(async function () {

    let image = document.getElementById("canvas").toDataURL("image/png");
    image = image.replace('data:image/png;base64,', '');
    
    let respons = await fetch(
      `../../../Admin/UploadImage`,
      {
          method: 'POST',
          body: JSON.stringify(image),
          headers: { 'content-type': 'application/json' }
      }
    );
    if (respons.status == 200) {
        location.reload();
    }

  });




