import config from '@/config';
import http from '@/utils/request';

export default {
    token: {
        url: `/connect/token`,
        name: '登录获取TOKEN',
        post: async function (data = {}) {
            let header = {
                headers: {
                    'Content-Type': 'application/x-www-form-urlencoded',
                },
            };
            return await http.post(this.url, data, header);
        },
    },
    getConfiguration: {
        // abp/application-configuration
        url: `${config.API_URL}/app/getConfiguration`,
        name: '获取用户配置（包括权限）',
        post: async function (data = {}) {
            return await http.post(`${this.url}?applicationId=${data.id}`);
        },
    },
};
