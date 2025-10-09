import argparse
from PIL import Image
import os
import math

def ensure_square(im):
    max_side = max(im.size)
    new_im = Image.new('RGB', (max_side, max_side), (0, 0, 0))
    #new_im.paste(im, ((max_side - im.width) // 2, (max_side - im.height) // 2))
    new_im.paste(im)
    return new_im

def save_tile(tile, z, x, y, output_dir, export_format, structure):
    # print(f'saving to folder {output_dir}')
    if structure == 'folder':
        path = os.path.join(output_dir, str(z), str(x))
        os.makedirs(path, exist_ok=True)
        tile.save(os.path.join(path, f'{y}.{export_format.lower()}'), export_format)
    else:
        os.makedirs(output_dir, exist_ok=True)
        tile.save(os.path.join(output_dir, f'{z}_{x}_{y}.{export_format.lower()}'), export_format)
    
def generate_tiles(im, tile_size, output_dir, export_format, structure):
    max_zoom = int(math.log2(im.size[0] / tile_size))
    for z in range(max_zoom + 1):
        scale = 2 ** (max_zoom - z)
        resized = im.resize((im.size[0] // scale, im.size[1] // scale), Image.LANCZOS)
        tiles_x = int(math.ceil(resized.size[0] // tile_size))
        tiles_y = int(math.ceil(resized.size[1] // tile_size))
                
        for x in range(tiles_x):
            for y in range(tiles_y):
                box = (x * tile_size, y * tile_size, (x + 1) * tile_size, (y + 1) * tile_size)
                tile = resized.crop(box)
                save_tile(tile, z, x, y, output_dir, export_format, structure)


# How to run:
# python tile_cutter.py your_image.jpg --tile_size 256 --output_dir output_tiles --format PNG --structure flat
def main():
    parser = argparse.ArgumentParser(description='Google Maps-style tile cutter using Pillow.')
    parser.add_argument('image', help='Path to input image')
    parser.add_argument('--tile_size', type=int, default=256, help='Size of each tile (default: 256)')
    parser.add_argument('--output_dir', default='tiles', help='Directory to save tiles (default: tiles)')
    parser.add_argument('--format', choices=['JPEG', 'PNG'], default='PNG', help='Export format (JPEG or PNG)')
    parser.add_argument('--structure', choices=['folder', 'flat'], default='folder', help='Export structure (folder or flat)')

    args = parser.parse_args()

    im = Image.open(args.image)
    im = ensure_square(im)
    
    generate_tiles(im, args.tile_size, args.output_dir, args.format, args.structure)
    print("Tile generation complete!")

if __name__ == '__main__':
    Image.MAX_IMAGE_PIXELS = 933120000
    main()

# FULL COMMAND:
#python GoogleMapsTileCutter.py "./out.png" --tile_size 256 --output_dir "./map/" --format PNG --structure folder
# WITH OMITED DEFAULTS
#python GoogleMapsTileCutter.py "./out.png" --output_dir "./map/"