// Physical Keygen for KW1 - by Nirav Patel <http://eclecti.cc>
// Revisions for KW10 - by Scott Elliott <mysd300.blogspot.com>
//
// Generate a duplicate of a Kwikset KW1 key by editing the last line of the file
// and entering in the key code of the lock.  If you don't know the key code,
// you can measure the key and compare the numbers at:
// http://web.archive.org/web/20050217020917fw_/http://dlaco.com/spacing/tips.htm
//
// This work is licensed under a Creative Commons Attribution 3.0 Unported License.

// Since the keys and locks I have were all designed in imperial units, the
// constants in this file will be defined in inches.  The mm function
// allows us to retain the proper size for exporting a metric STL.
function mm(i) = i*25.4;

module rounded(size, r) {
    union() {
        translate([r, 0, 0]) cube([size[0]-2*r, size[1], size[2]]);
        translate([0, r, 0]) cube([size[0], size[1]-2*r, size[2]]);
        translate([r, r, 0]) cylinder(h=size[2], r=r);
        translate([size[0]-r, r, 0]) cylinder(h=size[2], r=r);
        translate([r, size[1]-r, 0]) cylinder(h=size[2], r=r);
        translate([size[0]-r, size[1]-r, 0]) cylinder(h=size[2], r=r);
    }
}

module bit(bitw) {
    w = mm(1/4);
    difference() {
        translate([-w/2, 0, 0]) cube([w, mm(1), w]);
        translate([-mm(5/bitw), 0, 0]) rotate([0, 0, 135]) cube([w, w, w]);
        translate([mm(5/bitw), 0, 0]) rotate([0, 0, -45]) cube([w, w, w]);
    }
}

// Kwikset KW10, 6 pin key.  The measurements are mostly guesses based on reverse
// engineering some keys I have and some publicly available information.

module KW10(switch,bits) {

    usebitting = (switch == "code");
    makebump = (switch == "bump");


    // You may need to adjust these to fit your specific printer settings
    thickness = mm(0.080);
    length = mm(9/8)+mm(0.15); // add sixth pin
    width = mm(0.337);
    
    shoulder = mm(0.247);
    pin_spacing = mm(0.15);
    depth_inc = mm(0.023);
    
    // A fudge factor.  Printing with your average RepRap, the bottom layer is
    // going to be squeezed larger than you want. You can make the pins
    // go slighly deeper by a fudge amount to make up for it if you aren't
    // adjusting for it elsewhere like Skeinforge or in your firmware.
    fudge = 0.3; // try 0.5 for kit printers
    
    // Handle size
    h_l = mm(1);
    h_w = mm(1);
    h_d = mm(1/16);
    difference() {
        // blade and key handle
        union() {
            translate([-h_l, -h_w/2 + width/2, 0]) rounded([h_l, h_w, thickness], mm(1/4));
            // cut a little off the tip to avoid going too long
            cube([length - mm(1/64), width, thickness]);
        }
        
        // chamfer the tip
        translate([length, mm(1/8), 0]) {
            rotate([0, 0, 45]) cube([10, 10, thickness]);
            rotate([0, 0, 225]) cube([10, 10, thickness]);
        }
        
        // put in a hole for keychain use
        translate([-h_l + mm(3/16), width/2, 0]) cylinder(h=thickness, r=mm(1/8));
        
        // cut the channels in the key.  designed more for printability than accuracy
        union() {
            translate([-h_d, mm(0.105), mm(0.025)]) rotate([225, 0, 0]) cube([length + h_d, width, width]);
            translate([-h_d, mm(0.105), mm(0.05)]) rotate([260, 0, 0]) cube([length + h_d, thickness/2, mm(1/32)]);
            translate([-h_d, mm(0.105), 0]) cube([length + h_d, mm(7/128), mm(0.05)]);
            translate([-h_d, mm(0.105) + mm(7/128), mm(0.05)]) rotate([225, 0, 0]) cube([length + h_d, mm(3/64), thickness]);
        }
        
        translate([-h_d, width - mm(9/64), mm(0.043)]) {
            cube([length + h_d, width - (width - mm(10/64)), thickness]);
            rotate([50, 0, 0]) cube([length + h_d, width, thickness]);
        }
        
        union() {
            translate([-h_d, mm(0.015), mm(0.03)]) cube([length + h_d, mm(15/256), thickness]);
            translate([-h_d, mm(0.015) + mm(13/256), thickness - mm(1/64)]) rotate([45, 0, 0]) cube([length + h_d, mm(1/16), mm(1/16)]);
        }
        
      // Do the actual bitting
     if (usebitting) {
          for (b = [0:5]) {
              // Kwikset doesn't have a 0 value and instead has 1 start around .008" deep
              translate([shoulder + fudge + b*pin_spacing, width - mm(0.008) - (bits[b] - 1)*depth_inc - fudge, 0]) bit(128);
        }}

	  if (makebump) {
         // KW doesn't have a 0 value and instead has 1 start around .008" deep
         translate([shoulder + fudge + 0*pin_spacing, width - mm(.008) - (bits[0] - 1)*depth_inc - fudge, 0]) bit(512);
         translate([shoulder + fudge + 1*pin_spacing, width - mm(.008) - (bits[1] - 1)*depth_inc - fudge, 0]) bit(512);
         translate([shoulder + fudge + 2*pin_spacing, width - mm(.008) - (bits[2] - 1)*depth_inc - fudge, 0]) bit(512);
         translate([shoulder + fudge + 3*pin_spacing, width - mm(.008) - (bits[3] - 1)*depth_inc - fudge, 0]) bit(512);
         translate([shoulder + fudge + 4*pin_spacing, width - mm(.008) - (bits[4] - 1)*depth_inc - fudge, 0]) bit(512);
         translate([shoulder + fudge + 5*pin_spacing, width - mm(.008) - (bits[5] - 1)*depth_inc - fudge, 0]) bit(512);               
         translate([shoulder + fudge + 6*pin_spacing, width - mm(.008) - (bits[5] - 1)*depth_inc - fudge, 0]) bit(512);
       }

    }

}
// This sample key goes to a lock that is sitting disassembled on my desk
// Flip the key over for easy printing
// kw1([1,3,3,2,3]);

// Thickness from edge to keel:
// 1 = 7.96
// 2 = 7.31
// 3 = 6.84
// 4 = 6.26
// 5 = 5.67
// 6 = 5.09
// 7 = 4.50

//KW10("code",[9,9,9,9,9,9]);
KW10("bump",[7,7,7,7,7,7]);
