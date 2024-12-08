<template>
    <div class="user-bar">
        <div class="panel-item hidden-sm-and-down" @click="search">
            <el-icon><el-icon-search /></el-icon>
        </div>
        <div class="screen panel-item hidden-sm-and-down" @click="screen">
            <el-icon><el-icon-full-screen /></el-icon>
        </div>
        <el-dropdown class="user panel-item" trigger="click" @command="handleUser">
            <div class="user-avatar">
                <el-avatar :size="30">{{ userNameF }}</el-avatar>
                <label>{{ userName }}</label>
                <el-icon class="el-icon--right"><el-icon-arrow-down /></el-icon>
            </div>
            <template #dropdown>
                <el-dropdown-menu>
                    <el-dropdown-item command="uc">{{$t('user.accountInfo')}}</el-dropdown-item>
                    <el-dropdown-item command="clearCache">{{ $t('user.clearCache') }}</el-dropdown-item>
                    <el-dropdown-item divided command="outLogin">{{$t('user.outLogin')}}</el-dropdown-item>
                </el-dropdown-menu>
            </template>
        </el-dropdown>
    </div>

    <el-dialog v-model="searchVisible" :width="700" :title="$t('user.search')" center destroy-on-close>
        <search @success="searchVisible=false"></search>
    </el-dialog>

</template>

<script>
import search from './search.vue';

export default {
    components: {
        search,
    },
    data() {
        return {
            userName: '',
            userNameF: '',
            searchVisible: false,
            tasksVisible: false,
            msg: false,
        };
    },
    created() {
        var userInfo = this.$TOOL.data.get('USER_INFO');
        this.userName = userInfo.userName;
        this.userNameF = this.userName.substring(0, 1);
    },
    methods: {
        handleUser(command) {
            if (command == 'uc') {
                this.$router.push({ path: '/usercenter' });
            }
            if (command == 'cmd') {
                this.$router.push({ path: '/cmd' });
            }
            if (command == 'clearCache') {
                this.$confirm(
                    this.$t('user.clearCacheConfirm'),
                    this.$t('form.confirm'),
                    {
                        type: 'info',
                    }
                )
                    .then(() => {
                        const loading = this.$loading();
                        this.$TOOL.data.clear();
                        this.$router.replace({ path: '/login' });
                        setTimeout(() => {
                            loading.close();
                            location.reload();
                        }, 1000);
                    })
                    .catch(() => {});
            }
            if (command == 'outLogin') {
                this.$confirm(
                    this.$t('user.outLoginConfirm'),
                    this.$t('form.confirm'),
                    {
                        type: 'warning',
                        confirmButtonClass: 'el-button--danger',
                    }
                )
                    .then(() => {
                        this.$router.replace({ path: '/login' });
                    })
                    .catch(() => {});
            }
        },
        screen() {
            var element = document.documentElement;
            this.$TOOL.screen(element);
        },
        showMsg() {
            this.msg = true;
        },
        markRead() {
            this.msgList = [];
        },
        search() {
            this.searchVisible = true;
        },
    },
};
</script>

<style scoped>
.user-bar {
    display: flex;
    align-items: center;
    height: 100%;
}
.user-bar .panel-item {
    padding: 0 10px;
    cursor: pointer;
    height: 100%;
    display: flex;
    align-items: center;
}
.user-bar .panel-item i {
    font-size: 16px;
}
.user-bar .panel-item:hover {
    background: rgba(0, 0, 0, 0.1);
}
.user-bar .user-avatar {
    height: 49px;
    display: flex;
    align-items: center;
}
.user-bar .user-avatar label {
    display: inline-block;
    margin-left: 5px;
    font-size: 12px;
    cursor: pointer;
}
</style>
