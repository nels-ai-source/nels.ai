<template>
    <div v-if="navMenus.length<=0" style="padding:20px;">
        <el-alert title="无子集菜单" center type="info" :closable="false"></el-alert>
    </div>
    <template v-for="navMenu in navMenus" v-bind:key="navMenu">
        <el-menu-item v-if="!hasChildren(navMenu)" :index="navMenu.path" @click="handleClick(navMenu)">
            <a v-if="navMenu.meta&&navMenu.meta.type=='link'" :href="navMenu.path" target="_blank" @click.stop='()=>{}'></a>
            <el-icon v-if="navMenu.meta&&navMenu.meta.icon">
                <component :is="navMenu.meta.icon || 'el-icon-menu'" />
            </el-icon>
            <template #title>
                <span>{{navMenu.meta.title}}</span>
                <span v-if="navMenu.meta.tag" class="menu-tag">{{navMenu.meta.tag}}</span>
            </template>
        </el-menu-item>
        <el-sub-menu v-else :index="navMenu.path">
            <template #title>
                <el-icon v-if="navMenu.meta&&navMenu.meta.icon">
                    <component :is="navMenu.meta.icon || 'el-icon-menu'" />
                </el-icon>
                <span>{{navMenu.meta.title}}</span>
                <span v-if="navMenu.meta.tag" class="menu-tag">{{navMenu.meta.tag}}</span>
            </template>
            <NavMenu :navMenus="navMenu.children"></NavMenu>
        </el-sub-menu>
    </template>
</template>

<script>
export default {
    name: 'NavMenu',
    props: ['navMenus'],
    data() {
        return {};
    },
    methods: {
        hasChildren(item) {
            return (
                item.children &&
                !item.children.every((item) => item.meta.hidden)
            );
        },
        async handleClick(item) {
            if (item.parentId === '00000000-0000-0000-0000-000000000000') {
                //获取菜单
                await this.$API.auth.getConfiguration
                    .post({ id: item.id })
                    .then((res) => {
                        if (res.menus.length == 0) {
                            this.$alert(
                                '当前用户无任何菜单权限，请联系系统管理员',
                                '无权限访问',
                                {
                                    type: 'error',
                                    center: true,
                                }
                            );
                            return false;
                        }
                        this.$TOOL.data.set('USER_INFO', res.userInfo);
                        this.$TOOL.data.set('MENU', res.menus);
                        this.$TOOL.data.set('PERMISSIONS', res.permissions);
                        //this.$TOOL.data.set('DASHBOARDGRID',menu.data.dashboardGrid);
                        this.$router
                            .replace({
                                path: '/dashboard',
                            })
                            .then(() => {
                                this.$router.go(0);
                            });
                    });
            }
        },
    },
};
</script>
