using WebTool.Model;
using Microsoft.AspNetCore.Mvc;

using System.Drawing.Imaging;
using QRCoder;
using Newtonsoft.Json;
using Microsoft.IdentityModel.Tokens;
using System.Drawing.Drawing2D;
using System.Drawing;

namespace WebTool.Services
{
    public class QrCodeServices
    {
        public byte[] DecodeBase64String(string base64String)
        {
            // Loại bỏ phần đầu của chuỗi Base64 nếu có (ví dụ: "data:image/png;base64,")
            if (base64String.StartsWith("data:image/png;base64,"))
            {
                base64String = base64String.Substring("data:image/png;base64,".Length);
            }
            return Convert.FromBase64String(base64String);
        }

        public async Task<IActionResult> GenQrCode(QrcodeDto qrCode)
        {
            try
            {
                using (var qrCodeGenerator = new QRCodeGenerator())
                {
                    var qrCodeData = qrCodeGenerator.CreateQrCode(qrCode.TextData, QRCodeGenerator.ECCLevel.Q);
                    if (string.IsNullOrEmpty(qrCode.TextData))
                    {
                        var textCombine = $"SMSTO:{qrCode.PhoneNumber}:{qrCode.Message}";
                        qrCodeData = qrCodeGenerator.CreateQrCode(textCombine, QRCodeGenerator.ECCLevel.Q);
                    }
                    var qrCodeMatrix = qrCodeData.ModuleMatrix;
                    int moduleSize = 20;
                    int qrCodeWidth = qrCodeMatrix.Count * moduleSize;
                    int qrCodeHeightBonus;
                    if (qrCode.FramePosition != 0)
                    {
                        qrCodeHeightBonus = 90;
                    }
                    else
                    {
                        qrCodeHeightBonus = 0;
                    }
                    int qrCodeHeight = qrCodeMatrix.Count * moduleSize + qrCodeHeightBonus;

                    Bitmap qrCodeBitmap = new Bitmap(qrCodeWidth, qrCodeHeight);
                    using (Graphics graphics = Graphics.FromImage(qrCodeBitmap))
                    {
                        graphics.Clear(ColorTranslator.FromHtml(qrCode.BackgroundColor ?? "#FFFFFF"));
                        graphics.InterpolationMode = InterpolationMode.NearestNeighbor;
                        graphics.SmoothingMode = SmoothingMode.None;

                        Color darkColor = ColorTranslator.FromHtml(qrCode.ForegroundColor ?? "#000000");
                        int cornerSize = moduleSize * 1;
                        int yOffset;
                        if (qrCode.FramePosition == 1)
                        {
                            yOffset = 96;
                        }
                        else
                        {
                            yOffset = 0;
                        }
                        for (int y = 0; y < qrCodeMatrix.Count; y++)
                        {
                            for (int x = 0; x < qrCodeMatrix.Count; x++)
                            {
                                if (qrCodeMatrix[y][x])
                                {
                                    if (IsCorner(x, y, qrCodeMatrix.Count))
                                    {
                                        var cornerShape = qrCode.CornerShape?.ToLower() ?? "square";
                                        Color color = ColorTranslator.FromHtml(qrCode.CornerColor);
                                        switch (cornerShape)
                                        {
                                            case "circle":
                                                graphics.FillEllipse(new SolidBrush(color),
                                                    x * moduleSize, y * moduleSize + yOffset, cornerSize, cornerSize);
                                                break;

                                            case "rectangle":
                                            default:
                                                graphics.FillRectangle(new SolidBrush(color),
                                                    x * moduleSize, y * moduleSize + yOffset, cornerSize, cornerSize);
                                                break;

                                            case "parallelogram":
                                                Point[] parallelogramPoints = {
                                                    new Point(x * moduleSize, y * moduleSize + yOffset + cornerSize / 2),
                                                    new Point(x * moduleSize + cornerSize / 2, y * moduleSize + yOffset),
                                                    new Point(x * moduleSize + cornerSize + cornerSize / 2, y * moduleSize + yOffset),
                                                    new Point(x * moduleSize + cornerSize, y * moduleSize + yOffset + cornerSize / 2)
                                                };
                                                graphics.FillPolygon(new SolidBrush(color), parallelogramPoints);
                                                break;

                                            case "triangle":
                                                Point[] trianglePoints = {
                                                        new Point(x * moduleSize + cornerSize / 2, y * moduleSize + yOffset),
                                                        new Point(x * moduleSize + cornerSize, y * moduleSize + yOffset + cornerSize),
                                                        new Point(x * moduleSize, y * moduleSize + yOffset + cornerSize)
                                                    };
                                                graphics.FillPolygon(new SolidBrush(color), trianglePoints);
                                                break;

                                            case "oval":
                                                graphics.FillEllipse(new SolidBrush(color),
                                                    x * moduleSize, y * moduleSize + yOffset, cornerSize, cornerSize / 2);
                                                break;
                                        }
                                    }
                                    else
                                    {
                                        Color color = ColorTranslator.FromHtml(qrCode.DotColor);
                                        switch (qrCode.DotShape)
                                        {
                                            case "rectangle":
                                                graphics.FillRectangle(new SolidBrush(color),
                                                    x * moduleSize, y * moduleSize + yOffset, cornerSize, cornerSize);
                                                break;

                                            case "triangle":
                                                Point[] trianglePoints = {
                                                    new(x * moduleSize, y * moduleSize + yOffset),
                                                    new(x * moduleSize + moduleSize, y * moduleSize + yOffset),
                                                    new(x * moduleSize + moduleSize / 2, y * moduleSize + yOffset + moduleSize)
                                                };
                                                graphics.FillPolygon(new SolidBrush(color), trianglePoints);
                                                break;

                                            case "circle":
                                            default:
                                                graphics.FillEllipse(new SolidBrush(color),
                                                    x * moduleSize, y * moduleSize + yOffset, moduleSize, moduleSize);
                                                break;

                                            case "parallelogram":
                                                Point[] parallelogramPoints = {
                                                    new (x * moduleSize, y * moduleSize + yOffset),
                                                    new (x * moduleSize + moduleSize / 2, y * moduleSize + yOffset + moduleSize / 2),
                                                    new (x * moduleSize + moduleSize + moduleSize / 2, y * moduleSize + yOffset + moduleSize / 2),
                                                    new (x * moduleSize + moduleSize, y * moduleSize + yOffset)
                                                };
                                                graphics.FillPolygon(new SolidBrush(color), parallelogramPoints);
                                                break;
                                            case "oval":
                                                graphics.FillEllipse(new SolidBrush(color),
                                                    x * moduleSize, y * moduleSize + yOffset, (float)moduleSize, (float)(moduleSize * 1.5));
                                                break;
                                            case "flame":
                                                Point[] flamePoints = {
                                                    new(x * moduleSize + moduleSize / 2, y * moduleSize + yOffset), 
                                                    // Điểm đỉnh trên cùng
                                                    new(x * moduleSize, y * moduleSize + moduleSize / 2 + yOffset), 
                                                    // Điểm góc trái dưới
                                                    new(x * moduleSize + moduleSize / 4, y * moduleSize + moduleSize + yOffset), 
                                                    // Điểm trái dưới
                                                    new(x * moduleSize + moduleSize / 2, y * moduleSize + moduleSize / 2 + yOffset), 
                                                    // Điểm trái trên
                                                    new(x * moduleSize + moduleSize, y * moduleSize + moduleSize / 2 + yOffset), 
                                                    // Điểm phải dưới
                                                    new(x * moduleSize + moduleSize * 3 / 4, y * moduleSize + yOffset) 
                                                    // Điểm phải trên
                                                };
                                                graphics.FillPolygon(new SolidBrush(color), flamePoints);
                                                break;
                                        }
                                    }
                                }
                            }
                        }

                        if (!string.IsNullOrEmpty(qrCode.Logo))
                        {
                            byte[] logoBytes = DecodeBase64String(qrCode.Logo);
                            using (var logoStream = new MemoryStream(logoBytes))
                            using (var logo = Image.FromStream(logoStream))
                            {
                                var logoSize = Math.Min(qrCodeBitmap.Width, qrCodeBitmap.Height) / 6;
                                var logoX = (qrCodeBitmap.Width - logoSize) / 2;

                                var logoY = (qrCodeBitmap.Height + 90 - logoSize) / 2;
                                if (qrCode.FramePosition == 2)
                                {
                                    logoY = (qrCodeBitmap.Height - 68 - logoSize) / 2;
                                }
                                if (qrCode.FramePosition == 0)
                                {
                                    logoY = (qrCodeBitmap.Height - logoSize) / 2;
                                }


                                using (var logoBitmap = new Bitmap(logo, new Size(logoSize, logoSize)))
                                {
                                    graphics.DrawImage(logoBitmap, new Rectangle(logoX, logoY, logoSize, logoSize));
                                }
                            }
                        }

                        if (qrCode.FramePosition != 0)
                        {
                            var colorFrame = qrCode.ColorFrame;
                            Color color = ColorTranslator.FromHtml(colorFrame);
                            var colorPath = "#FE0C0C";
                            Color colorPathh = ColorTranslator.FromHtml(colorPath);
                            var frameThickness = 50; // Độ dày của khung các cạnh khác
                            var bottomFrameThickness = 120; // Độ dày của khung ở đáy
                                                            // var radius = 20;
                            // Vẽ khung các cạnh khác
                            using (var path = new GraphicsPath())
                            {
                                int radius = 60; // Độ bo tròn của các góc
                                int diameter = radius * 2; // Đường kính của góc bo tròn

                                // Thêm các góc bo tròn vào path
                                if (qrCode.FramePosition == 2)
                                {
                                    path.AddArc(0, 0, diameter, diameter, 180, 90); // Góc trên trái
                                    path.AddArc(qrCodeBitmap.Width - diameter, 0, diameter, diameter, 270, 90); // Góc trên phải
                                    path.AddArc(qrCodeBitmap.Width - diameter, qrCodeBitmap.Height - diameter - 100, diameter, diameter, 0, 90); // Góc dưới phải
                                    path.AddArc(0, qrCodeBitmap.Height - diameter - 100, diameter, diameter, 90, 90); 
                                    // Góc dưới trái
                                }
                                else
                                {
                                    path.AddArc(0, 100, diameter, diameter, 180, 90); // Góc trên trái
                                    path.AddArc(qrCodeBitmap.Width - diameter, 100, diameter, diameter, 270, 90); // Góc trên phải
                                    path.AddArc(qrCodeBitmap.Width - diameter, qrCodeBitmap.Height - diameter, diameter, diameter, 0, 90); // Góc dưới phải
                                    path.AddArc(0, qrCodeBitmap.Height - diameter, diameter, diameter, 90, 90); // Góc dưới trái
                                }

                                path.CloseFigure(); // Đóng đường path

                                using (var pen = new Pen(color, frameThickness))
                                {
                                    graphics.DrawPath(pen, path);
                                }
                            }
                            // Vẽ khung dày ở đáy
                            using (var bottomPen = new Pen(color, bottomFrameThickness))
                            {
                                if (qrCode.FramePosition == 1)
                                {
                                    graphics.DrawLine(bottomPen, 0, 60, qrCodeBitmap.Width, 60);
                                }
                                else
                                {
                                    graphics.DrawLine(bottomPen, 0, qrCodeBitmap.Height - 60, qrCodeBitmap.Width, qrCodeBitmap.Height - 60);
                                }
                            }
                            // Vẽ chữ vào khung
                            if (!string.IsNullOrEmpty(qrCode.FrameText))
                            {
                                var colorText = qrCode.ColorText;
                                Color myColorText = ColorTranslator.FromHtml(colorText);
                                var font = new Font(qrCode.TextFont, 40, FontStyle.Bold);
                                var brush = new SolidBrush(myColorText);
                                // Tính toán vị trí để chèn chữ vào khung
                                var textSize = graphics.MeasureString(qrCode.FrameText, font);
                                var TextX = (qrCodeBitmap.Width - textSize.Width) / 2;
                                var TextY = qrCodeBitmap.Height - 86;
                                if (qrCode.FramePosition != 2)
                                {
                                    TextX = (qrCodeBitmap.Width - textSize.Width) / 2;
                                    TextY = 26;
                                }
                                graphics.DrawString(qrCode.FrameText, font, brush, new PointF(TextX, TextY));
                            }
                        }

                        using (var stream = new MemoryStream())
                        {
                            qrCodeBitmap.Save(stream, ImageFormat.Png);
                            return new FileContentResult(stream.ToArray(), "image/png");
                        }
                    }
                }
            }
            catch (Exception)
            {
                return new StatusCodeResult(500);
            }
        }

        public bool IsCorner(int x, int y, int matrixSize)
        {
            // Kiểm tra các góc của QR code
            int margin = 11; // Độ lớn của vùng góc để không vẽ chấm

            // Góc trên trái
            if (x < margin && y < margin) return true;
            // Góc trên phải
            if (x >= matrixSize - margin && y < margin) return true;
            // Góc dưới trái
            if (x < margin && y >= matrixSize - margin) return true;

            return false;
        }
    }

}
