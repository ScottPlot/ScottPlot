using System.Text;

namespace WinForms_Demo;

public class HtmlMenu
{
    private readonly StringBuilder Content = new();

    public HtmlMenu()
    {

    }

    public void Add(string title, string description)
    {
        Content.AppendLine($"<div class='card' onclick=\"postIt('{title}')\">");
        Content.AppendLine($"  <div class='card-title'>{title}</div>");
        Content.AppendLine($"  <div>{description}</div>");
        Content.AppendLine($"</div>");
    }

    public string GetEverything()
    {
        return @"
        <html>

        <head>
            <style>
                body {
                    background-color: #67217a;
                    font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
                    color: white;
                }

                .banner {
                    display: flex;
                }

                .banner-icon {
                    width: 64px;
                    height: 64px;
                    background-size: 64px 64px;
                    margin: .5em;
                    background-image: url('data:image/svg+xml,%3C%3Fxml version='1.0' encoding='UTF-8' standalone='no'%3F%3E%3Csvg xmlns:dc='http://purl.org/dc/elements/1.1/' xmlns:cc='http://creativecommons.org/ns%23' xmlns:rdf='http://www.w3.org/1999/02/22-rdf-syntax-ns%23' xmlns:svg='http://www.w3.org/2000/svg' xmlns='http://www.w3.org/2000/svg' xmlns:sodipodi='http://sodipodi.sourceforge.net/DTD/sodipodi-0.dtd' xmlns:inkscape='http://www.inkscape.org/namespaces/inkscape' width='512' height='512' viewBox='0 0 135.46666 135.46667' version='1.1' id='svg8' inkscape:version='1.0.2-2 (e86c870879, 2021-01-15)' sodipodi:docname='scottplot-icon-rounded-border.svg' inkscape:export-filename='C:%5CUsers%5Cscott%5CDocuments%5CGitHub%5CScottPlot%5Cdev%5Cicon%5Cv5%5Cscottplot-icon-rounded-border-128.png' inkscape:export-xdpi='24' inkscape:export-ydpi='24'%3E%3Cdefs id='defs2' /%3E%3Csodipodi:namedview id='base' pagecolor='%23ffffff' bordercolor='%23666666' borderopacity='1.0' inkscape:pageopacity='0.0' inkscape:pageshadow='2' inkscape:zoom='1' inkscape:cx='145.65903' inkscape:cy='328.66889' inkscape:document-units='mm' inkscape:current-layer='layer1' inkscape:document-rotation='0' showgrid='false' units='px' inkscape:pagecheckerboard='true' borderlayer='true' inkscape:showpageshadow='true' inkscape:window-width='1920' inkscape:window-height='1018' inkscape:window-x='1912' inkscape:window-y='-8' inkscape:window-maximized='1' showborder='true'%3E%3Cinkscape:grid type='xygrid' id='grid833' spacingx='8.4666666' spacingy='8.4666666' /%3E%3C/sodipodi:namedview%3E%3Cmetadata id='metadata5'%3E%3Crdf:RDF%3E%3Ccc:Work rdf:about=''%3E%3Cdc:format%3Eimage/svg+xml%3C/dc:format%3E%3Cdc:type rdf:resource='http://purl.org/dc/dcmitype/StillImage' /%3E%3Cdc:title /%3E%3C/cc:Work%3E%3C/rdf:RDF%3E%3C/metadata%3E%3Cg inkscape:label='Layer 1' inkscape:groupmode='layer' id='layer1'%3E%3Cpath id='rect835' style='opacity:1;fill:%2367217a;fill-opacity:1;stroke:%23ffffff;stroke-width:0.755906;stroke-linecap:round;stroke-linejoin:round;stroke-opacity:0.0645161' d='M 128 0 C 57.088004 0 0 57.088004 0 128 L 0 362.88672 L 147.19531 207.47656 L 263.32617 289.15625 L 488.67188 54.134766 C 465.53924 21.342617 427.35552 0 384 0 L 128 0 z M 512 139.02734 L 272.95312 388.33984 L 157.14648 306.88867 L 19.544922 452.17773 C 42.151668 488.16713 82.186824 512 128 512 L 384 512 C 454.912 512 512 454.912 512 384 L 512 139.02734 z ' transform='scale(0.26458333)' /%3E%3Cpath id='rect835-5' style='fill:%239a4993;fill-opacity:1;stroke:none;stroke-width:0.755906;stroke-linecap:round;stroke-linejoin:round;stroke-opacity:1' d='M 512 139.02344 L 272.95312 388.33984 L 157.14648 306.88672 L 19.544922 452.17773 C 42.151668 488.16713 82.186825 512 128 512 L 384 512 C 454.91199 512 512 454.91203 512 384 L 512 139.02344 z ' transform='scale(0.26458333)' /%3E%3Cpath id='rect835-8' style='fill:%23ffffff;fill-opacity:1;stroke:%23ffffff;stroke-width:0.2;stroke-linecap:round;stroke-linejoin:round;stroke-opacity:0.0645161' d='M 129.29392,14.323157 69.671715,76.505924 38.944909,54.894839 -5.1858333e-4,96.013777 V 101.6 c 0,6.64073 1.89435848333,12.82194 5.17125998333,18.03869 L 41.577823,81.197626 72.218329,102.74825 135.46615,36.784317 v -2.917651 c 0,-7.290985 -2.28209,-14.028938 -6.17223,-19.543509 z' /%3E%3Cpath style='color:%23000000;font-style:normal;font-variant:normal;font-weight:normal;font-stretch:normal;font-size:medium;line-height:normal;font-family:sans-serif;font-variant-ligatures:normal;font-variant-position:normal;font-variant-caps:normal;font-variant-numeric:normal;font-variant-alternates:normal;font-variant-east-asian:normal;font-feature-settings:normal;font-variation-settings:normal;text-indent:0;text-align:start;text-decoration:none;text-decoration-line:none;text-decoration-style:solid;text-decoration-color:%23000000;letter-spacing:normal;word-spacing:normal;text-transform:none;writing-mode:lr-tb;direction:ltr;text-orientation:mixed;dominant-baseline:auto;baseline-shift:baseline;text-anchor:start;white-space:normal;shape-padding:0;shape-margin:0;inline-size:0;clip-rule:nonzero;display:inline;overflow:visible;visibility:visible;opacity:1;isolation:auto;mix-blend-mode:normal;color-interpolation:sRGB;color-interpolation-filters:linearRGB;solid-color:%23000000;solid-opacity:1;vector-effect:none;fill:%239a4993;fill-opacity:1;fill-rule:nonzero;stroke:none;stroke-linecap:butt;stroke-linejoin:round;stroke-miterlimit:4;stroke-dasharray:none;stroke-dashoffset:0;stroke-opacity:1;paint-order:fill markers stroke;color-rendering:auto;image-rendering:auto;shape-rendering:auto;text-rendering:auto;enable-background:accumulate;stop-color:%23000000;stop-opacity:1' d='m 29.251823,0.05846355 c -16.086644,0 -29.19335945,13.10671545 -29.19335945,29.19335945 v 76.963147 c 0,16.08664 13.10671345,29.19336 29.19335945,29.19336 h 76.963147 c 16.08665,0 29.72253,-13.10671 29.72253,-29.19336 V 29.251823 c 0,-16.086646 -13.63589,-29.19335945 -29.72253,-29.19335945 z M 30.310156,10.058464 h 75.904814 c 10.71962,0 19.72253,9.00291 19.72253,19.722526 v 75.37565 c 0,10.71962 -10.06124,20.25169 -20.78086,20.25169 H 30.310156 c -10.719616,0 -20.251692,-10.5904 -20.251692,-21.31002 V 30.310156 c 0,-10.719617 9.532075,-20.251692 20.251692,-20.251692 z' id='rect834' sodipodi:nodetypes='ssssssssssssssssss' /%3E%3C/g%3E%3C/svg%3E%0A');
                }

                .banner-title {
                    margin-top: .15em;
                    font-size: 2.5em;
                    line-height: 100%;
                }

                .banner-subtitle {
                    font-size: 1.5em;
                    font-weight: 300;
                    line-height: 100%;
                    color: rgba(255, 255, 255, .5);
                }

                .card {
                    margin: .5em;
                    margin-bottom: 1em;
                    background-color: rgba(154, 73, 147, .3);
                    padding: 1em;
                    border-radius: .75em;
                    color: rgba(255, 255, 255, .8);
                    transition: 0.1s;
                }

                .card:hover {
                    background-color: rgba(154, 73, 147, 1.0);
                    color: white;
                    transition: 0.1s;
                    cursor: pointer;
                }

                .card-title {
                    font-size: 1.5em;
                }
            </style>
            <script>
                function postIt(title) {
                    console.log('posting: ' + title);
                    window.chrome.webview.postMessage(title);
                }
            </script>
        </head>

        <body>

            <div class='banner'>
                <img class='banner-icon'
                    src='data:image/png;base64,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' />
                <div>
                    <div class='banner-title'>ScottPlot 5.0.0</div>
                    <div class='banner-subtitle'>Windows Forms Demo</div>
                </div>
            </div>

            <section class='cards'>
            " +
            Content.ToString()
            + @"</section></body></html>";
    }
}
