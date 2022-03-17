
[![Docker Image CI](https://github.com/huseyinorer/netCore/actions/workflows/docker-image.yml/badge.svg)](https://github.com/huseyinorer/netCore/actions/workflows/docker-image.yml)

Uygulama kabaca kişisel yönetilebilir satış sayfası olacaktır. 
Üyelik sistemi barındırmaktadır (google,facebook).
Veritabanı olarak postgresql kullanmakta.
Uygulama .net 5

CI/CD işlemleri
Uygulama hem bitbucked hem github'da depolanmaktadır. Birinde pipeline diğerinde action ile Push'tan sonra aşağıdaki işlemleri yapmaktadır.
- Uygulama derlenir
- Paketler oluşturulup dockerhub'a gönderilir.
- uzak sunucuya ssh üzerinden bağlanıp docker-compose up yapılır ve 80 portundan uygulama ayağa kalkar.

Uzak sunucu 
- Ubuntu 20.04 lts
- Uygulamamızı nginx arkasına koyuyoruz (bunu dockerfile ve nginx.configinde belirtmemiz gerekli) 

