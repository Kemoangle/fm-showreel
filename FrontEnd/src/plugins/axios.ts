import axios from 'axios';

const axiosIns = axios.create({
    baseURL: 'http://localhost:5124/api/',
    headers: {
        // 'Access-Control-Allow-Credentials': 'true',
        // 'Access-Control-Allow-Methods': '*',
        // 'Access-Control-Allow-Origin': 'http://localhost:5173',
    },
});

axiosIns.interceptors.request.use((config) => {
    const token = localStorage.getItem('accessToken');

    if (token && config.headers) config.headers.Authorization = `Bearer ${token}`;

    return config;
});

axiosIns.interceptors.response.use(
    (originalResponse) => {
        return originalResponse.data;
    },
    async (error) => {
        console.log(error);
        if (error.response?.status === 400)
            // Bad request
            return Promise.resolve(error.response);

        if (error.response?.status === 403 || error.response?.status === 401) {
            // Forbidden or unauthorized
            const router = useRouter();

            await router.push('/not-authorized');
        }
    }
);

export default axiosIns;
