<template>
    <el-form ref="loginForm" :model="form" :rules="rules" label-width="0" size="large" @keyup.enter="login">
        <el-form-item prop="user">
            <el-input v-model="form.user" prefix-icon="el-icon-user" clearable :placeholder="$t('login.userPlaceholder')">
                <template #append>
                    <el-select v-model="userType" style="width: 130px;">
                        <el-option :label="$t('login.admin')" value="admin"></el-option>
                        <el-option :label="$t('login.user')" value="user"></el-option>
                    </el-select>
                </template>
            </el-input>
        </el-form-item>
        <el-form-item prop="password">
            <el-input v-model="form.password" prefix-icon="el-icon-lock" clearable show-password :placeholder="$t('login.PWPlaceholder')"></el-input>
        </el-form-item>
        <el-form-item style="margin-bottom: 10px;">
            <el-col :span="12">
                <el-checkbox :label="$t('login.rememberMe')" v-model="form.autologin"></el-checkbox>
            </el-col>
            <el-col :span="12" class="login-forgot">
                <router-link to="/reset_password">{{ $t('login.forgetPassword') }}？</router-link>
            </el-col>
        </el-form-item>
        <el-form-item>
            <el-button type="primary" style="width: 100%;" :loading="islogin" round @click="login">{{ $t('login.signIn') }}</el-button>
        </el-form-item>
        <div class="login-reg">
            {{$t('login.noAccount')}} <router-link to="/user_register">{{$t('login.createAccount')}}</router-link>
        </div>
    </el-form>
</template>

<script>
export default {
    data() {
        return {
            userType: 'admin',
            form: {
                user: 'admin',
                password: '1q2w3E*',
                autologin: false,
            },
            rules: {
                user: [
                    {
                        required: true,
                        message: this.$t('login.userError'),
                        trigger: 'blur',
                    },
                ],
                password: [
                    {
                        required: true,
                        message: this.$t('login.PWError'),
                        trigger: 'blur',
                    },
                ],
            },
            islogin: false,
        };
    },
    watch: {
        userType(val) {
            if (val == 'admin') {
                this.form.user = 'admin';
                this.form.password = 'admin';
            } else if (val == 'user') {
                this.form.user = 'user';
                this.form.password = 'user';
            }
        },
    },
    mounted() {},
    methods: {
        async login() {
            var validate = await this.$refs.loginForm
                .validate()
                .catch(() => {});
            if (!validate) {
                return false;
            }

            this.islogin = true;
            var data = {
                client_id: this.$CONFIG.CLIENT_ID,
                grant_type: this.$CONFIG.GRANT_TYPE,
                client_secret: this.$CONFIG.CLIENT_SECRET,
                scope: this.$CONFIG.SCOPE,
                username: this.form.user,
                password: this.form.password,
            };
            var login = await this.$API.auth.token
                .post(data)
                .then((res) => {
                    this.$TOOL.cookie.set('TOKEN', res.access_token, {
                        expires: this.form.autologin ? 24 * 60 * 60 : 0,
                    });
                    return true;
                })
                .catch(() => {
                    this.islogin = false;
                    return false;
                });
            if (!login) return false;

            await this.getConfig();

            this.$router.replace({
                path: '/',
            });
            this.islogin = false;
        },
        async getConfig() {
            await this.$API.auth.getConfiguration
                .post({ id: '00000000-0000-0000-0000-000000000000' })
                .then((res) => {
                    if (res.menus.length == 0) {
                        this.$alert(
                            this.$t('login.noMenus'),
                            this.$t('login.noPermission'),
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
                })
                .catch((err) => {
                    this.islogin = false;
                    this.$messageNels.warning(err.data.error.message);
                });
        },
    },
};
</script>

<style>
</style>
