stem_height = 16.6;
stem_r = 2.8;
hole_height = 6.5;
hole_r = 1.95;

teeth_height = 4;
tooth_width = 2.4;
tooth_height = 1;
base_depth = 2.2;
tooth_depth = 3.5;
tooth_gap = 1;

// Offset from the center of the stem
teeth_offset = 2.53 * 1;

rotate([0,90,0])
union() {
  difference() {
    stem();
    hole();
  }
  rounded_teeth();
  handle();
}

module stem() {
  $fn = 45;
  cylinder(r=stem_r, h=stem_height);
}

module hole() {
  $fn = 30;
  union() {
    cylinder(r=hole_r, h=hole_height-1);
    translate([0, 0, hole_height-1])
      cylinder(r1=hole_r, r2=hole_r*0.66, h=1);
  }
}

module rounded_teeth() {
  $fn = 90;
  intersection() {
    teeth();
    cylinder(r=teeth_offset+tooth_depth, h=teeth_height);
  }
}

module teeth() {
  translate([-tooth_width/2, teeth_offset, 0])
  union() {
    cube([tooth_width, base_depth, teeth_height]);
    cube([tooth_width, tooth_depth, tooth_height]);
    translate([0, 0, tooth_height+tooth_gap])
      cube([tooth_width, tooth_depth, tooth_height]);
  }
}

module handle() {
  $fn = 90;

  union() {
    translate([0, 0, stem_height+5.3])
      rotate([0, 90, 0])
      difference() {
        cylinder(r=6, h=2.5, center=true);
        cylinder(r=3.2, h=2.5, center=true);
      }
    translate([0, 0, stem_height-0.01])
      cylinder(r1=stem_r, r2=0, h=2);
  }
}

module original() {
  rotate([0,0,180])
    translate([0,0,11.68])
    import("Original.stl", convexity=10);
}
