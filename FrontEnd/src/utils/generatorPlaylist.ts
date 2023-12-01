import { IPlaylist, IVideos } from '@/model/generatorPlaylist';
import _ from 'lodash';
export class generatorPlaylist {
    createVideo = (data: IVideos) => {
        const newArr = [];
        for (let i = 0; i < data.loop; i++) {
            newArr.push(data);
        }
        return newArr;
    };

    checkCategoriesCloselyTogether = (listVideo: IVideos[] | IPlaylist[]) => {
        const indexVideo = listVideo.findIndex((x, i) => {
            if (i == listVideo.length - 1) {
                return x.category == listVideo[0].category && x.category && listVideo[0].category;
            } else {
                return (
                    x.category == listVideo[i + 1].category &&
                    x.category &&
                    listVideo[i + 1].category
                );
            }
        });
        if (indexVideo >= 0) {
            return true;
        }
        return false;
    };

    checkNoBackToBack = (listVideo: IVideos[]) => {
        for (let i = 0; i < listVideo.length; i++) {
            const element = listVideo[i];
            if (element?.restriction) {
                if (element?.restriction.includes('No Back to Back With ')) {
                    const btbName = element?.restriction.split(' With ')[1];
                    switch (btbName) {
                        case 'FMSG Contents':
                            if (
                                listVideo[i - 1]?.name.includes('FMSG') ||
                                listVideo[i + 1]?.name.includes('FMSG')
                            ) {
                                return {
                                    status: true,
                                    index: i,
                                    indexReject: listVideo[i - 1]?.name.includes('FMSG')
                                        ? i - 1
                                        : i + 1,
                                };
                            }
                            break;
                        case 'Crystal Tomato':
                            if (
                                listVideo[i - 1]?.name.includes('Crystal Tomato') ||
                                listVideo[i + 1]?.name.includes('Crystal Tomato')
                            ) {
                                return {
                                    status: true,
                                    index: i,
                                    indexReject: listVideo[i - 1]?.name.includes('Crystal Tomato')
                                        ? i - 1
                                        : i + 1,
                                };
                            }
                            break;

                        default:
                            break;
                    }
                }
            }
        }
        return { status: false, index: 0 };
    };

    handleCategoriesCloselyTogether = (listVideo: IVideos[]) => {
        const indexVideo = listVideo.findIndex((x, i) => {
            if (i == listVideo.length - 1) {
                return x.category == listVideo[0].category && x.category && listVideo[0].category;
            } else {
                return (
                    x.category == listVideo[i + 1].category &&
                    x.category &&
                    listVideo[i + 1].category
                );
            }
        });

        if (indexVideo >= 0) {
            const indexSwap = listVideo.findIndex((x, i) => {
                if (i == 0) {
                    return (
                        x.category !== listVideo[indexVideo].category &&
                        listVideo[indexVideo].category !== listVideo[i + 1].category &&
                        listVideo[indexVideo].category !== listVideo[listVideo.length - 1].category
                    );
                } else {
                    return (
                        x.category !== listVideo[indexVideo].category &&
                        listVideo[indexVideo].category !== listVideo[i + 1].category &&
                        listVideo[indexVideo].category !== listVideo[i - 1].category
                    );
                }
            });

            const newListVideo = [...listVideo];

            let swapVideo: IVideos = _.clone(newListVideo[indexVideo]);
            newListVideo[indexVideo] = _.clone(newListVideo[indexSwap]);
            newListVideo[indexSwap] = _.clone(swapVideo);

            return newListVideo;
        }
        return listVideo;
    };

    handleBackToBack = (
        listVideo: IVideos[],
        indexVideo: number,
        indexReject: number | undefined
    ) => {
        const newListVideo = [...listVideo];
        if (indexReject && indexVideo) {
            const indexSwap = listVideo.findIndex((x, i) => {
                if (i == 0) {
                    return (
                        x?.category !== listVideo[indexReject]?.category &&
                        listVideo[indexReject]?.category !== listVideo[i + 1]?.category &&
                        listVideo[indexReject]?.category !==
                            listVideo[listVideo?.length - 1]?.category &&
                        listVideo[indexReject]?.name !== listVideo[i + 1]?.name &&
                        listVideo[indexReject]?.name !== listVideo[listVideo?.length - 1].name &&
                        i !== indexVideo &&
                        i !== indexVideo + 1 &&
                        i !== indexVideo - 1
                    );
                } else {
                    if (i < listVideo.length - 1) {
                        if (listVideo[indexReject]?.category) {
                            return (
                                x?.category !== listVideo[indexReject]?.category &&
                                listVideo[indexReject]?.category !== listVideo[i + 1]?.category &&
                                listVideo[indexReject]?.category !== listVideo[i - 1]?.category &&
                                listVideo[indexReject]?.name !== listVideo[i - 1]?.name &&
                                i !== indexVideo &&
                                i !== indexVideo + 1 &&
                                i !== indexVideo - 1
                            );
                        } else {
                            return (
                                listVideo[indexReject]?.name !== listVideo[i - 1]?.name &&
                                i !== indexVideo &&
                                i !== indexVideo + 1 &&
                                i !== indexVideo - 1
                            );
                        }
                    }
                    return true;
                }
            });

            if (indexSwap >= -1) {
                const swapVideo: IVideos = _.clone(newListVideo[indexReject]);
                newListVideo[indexReject] = _.clone(newListVideo[indexSwap]);
                newListVideo[indexSwap] = _.clone(swapVideo);

                return newListVideo;
            }
        }
        return listVideo;
    };

    handleCheckAndSortCategoriesCloselyTogether = (listVideo: IVideos[]) => {
        let isCategoriesCloselyTogether = this.checkCategoriesCloselyTogether(listVideo);

        let newListVideo: IVideos[] = [];
        let i = 0;
        do {
            i++;
            newListVideo = this.handleCategoriesCloselyTogether(
                newListVideo.length ? newListVideo : listVideo
            );
            isCategoriesCloselyTogether = this.checkCategoriesCloselyTogether(newListVideo);
        } while (isCategoriesCloselyTogether && i < 20);

        return newListVideo;
    };

    handleCheckAndSortNoBackToBack = (listVideo: IVideos[]) => {
        let valueCheckNoBackToBack = this.checkNoBackToBack(listVideo);

        let newListVideo: IVideos[] = [];
        let i = 0;
        do {
            i++;
            newListVideo = this.handleBackToBack(
                newListVideo.length ? newListVideo : listVideo,
                valueCheckNoBackToBack.index,
                valueCheckNoBackToBack.indexReject
            );

            valueCheckNoBackToBack = this.checkNoBackToBack(newListVideo);
        } while (valueCheckNoBackToBack.status && i < 20);

        return newListVideo;
    };

    handleCustomeVideo = (listVideo: IVideos[]) => {
        const newList: IVideos[] = [];
        listVideo.forEach((video) => {
            newList.push(video);
        });
        return newList;
    };

    handleRemoveVideos = (listVideo: IVideos[]) => {
        const newList: IVideos[] = [];
        listVideo.forEach((video) => {
            if (video.loop && video.loop > 1) {
                newList.push({ ...video, loop: video.loop - 1 });
            }
        });
        return newList;
    };

    addLandLordAds = (listVideo: IVideos[], videos: IVideos[]) => {
        let newListVideo: IVideos[] = [...listVideo];
        const newVideosAds = [...videos];
        const loop = newVideosAds.reduce((prev, currentValue) => prev + currentValue.loop, 0);

        const index = +((listVideo.length + loop) / (loop + 1)).toFixed(0);

        let indexVideoAds = 0;

        for (let i = 0; i < loop; i++) {
            newListVideo.splice(index * (i + 1), 0, videos[indexVideoAds]);
            const loopVideo = newVideosAds[indexVideoAds].loop - 1;
            newVideosAds[indexVideoAds].loop = newVideosAds[indexVideoAds].loop - 1;
            if (loopVideo == 0) {
                indexVideoAds++;
            }
        }
        newListVideo = this.handleCheckAndSortCategoriesCloselyTogether(newListVideo);
        newListVideo = this.handleCheckAndSortNoBackToBack(newListVideo);
        return newListVideo;
    };

    createListVideo = (listVideo: IVideos[]) => {
        let oldListVideo = [...listVideo];

        oldListVideo.sort((a, b) => {
            if (a.loop && b.loop) {
                return b.loop - a.loop;
            }
            return 0;
        });

        let newListVideo = [];
        let i = 0;
        do {
            i++;
            newListVideo.push(...this.handleCustomeVideo(oldListVideo));
            oldListVideo = this.handleRemoveVideos(oldListVideo);
        } while (oldListVideo.length && i < 20);

        newListVideo = this.handleCheckAndSortCategoriesCloselyTogether(newListVideo);
        newListVideo = this.handleCheckAndSortNoBackToBack(newListVideo);

        return newListVideo;
    };

    handleRestrictionBuilding = (building: any, videos: IVideos[]) => {
        console.log('videos:', videos);
        console.log('building:', building);
    };
}
