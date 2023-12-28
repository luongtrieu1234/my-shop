# my-shop
# Project MyShop Windows
### Setup
1. Chạy file db.sql trong MSSQL 
2. Vào ProjectMyShop/Config, copy file App.config.dev ra 1 file, đặt tên file đó là App.config(file này local dưới máy) -> sửa các thông tin về password, username lại cho đúng
3. Bấm Ctrl + B để build
4. Nhấn F5 để chạy (không nhấn Ctrl+F5 sẽ không thấy lỗi) -> Ra một lỗi đường dẫn (ví dụ: không tìm thấy đường dẫn "D:/code/Assets/....)
5. Vào source code copy thư mục Assets bỏ vào thư mục không tìm thấy (ví dụ copy Assets bỏ vào D:/code , tùy máy sẽ lỗi khác nhau) -> chưa fix được, những không ảnh hưởng tới code khác
6. Login vào.
7. Chức năng import csv
  - Giải nén thư mục Images.zip trong Data ra. Copy thư mục bỏ vào ổ D thì mới chạy được  