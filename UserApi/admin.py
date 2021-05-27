from django.contrib import admin
from TbspRpgAdmin.admin import MultiDBModelAdmin
from .models import User
from .models import EfMigrationHistory

# Register your models here.
class MultiDBModelAdminUserApi(MultiDBModelAdmin):
    using = 'userapi'

admin.site.register(User, MultiDBModelAdminUserApi)
admin.site.register(EfMigrationHistory, MultiDBModelAdminUserApi)
