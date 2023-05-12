/////////////////////////////////////////////////////////////////////////////////////////////////////
//
// Audiokinetic Wwise generated include file. Do not edit.
//
/////////////////////////////////////////////////////////////////////////////////////////////////////

#ifndef __WWISE_IDS_H__
#define __WWISE_IDS_H__

#include <AK/SoundEngine/Common/AkTypes.h>

namespace AK
{
    namespace EVENTS
    {
        static const AkUniqueID AMBIENCESTART = 1720073155U;
        static const AkUniqueID ENEMYDIE = 794424115U;
        static const AkUniqueID ENEMYSHOOT = 2130386168U;
        static const AkUniqueID FANPLATFORM = 1922649833U;
        static const AkUniqueID FLIPPERPLATFORM = 1981223316U;
        static const AkUniqueID FOOTSTEPSTART = 824192661U;
        static const AkUniqueID FOOTSTEPSTOP = 2064397943U;
        static const AkUniqueID HEALTHREGEN = 686534402U;
        static const AkUniqueID JUMP = 3833651337U;
        static const AkUniqueID JUMPDOUBLE = 610395762U;
        static const AkUniqueID KEYPICKUP = 1616845350U;
        static const AkUniqueID LIGHTBUFF = 2199372872U;
        static const AkUniqueID LIGHTBUFFDOWN = 657083948U;
        static const AkUniqueID LIGHTSHIELDDOWN = 3201278584U;
        static const AkUniqueID LIGHTSHIELDUP = 3900558107U;
        static const AkUniqueID LOWHEALTH = 1017222595U;
        static const AkUniqueID MOVINGPLATFORM = 1948881314U;
        static const AkUniqueID ONCLICK = 21544190U;
        static const AkUniqueID ONHOVER = 3784259780U;
        static const AkUniqueID PAUSE = 3092587493U;
        static const AkUniqueID PLAYERAMBIENCESTART = 3045495790U;
        static const AkUniqueID PLAYERDEAD = 2356585300U;
        static const AkUniqueID PLAYERRESET = 644341835U;
        static const AkUniqueID PROJECTORON = 4294385764U;
        static const AkUniqueID RELOAD = 456382354U;
        static const AkUniqueID RELOADDONE = 4051456586U;
        static const AkUniqueID SHADOWBUFFEND = 3683447373U;
        static const AkUniqueID SHADOWBUFFSTART = 2416431302U;
        static const AkUniqueID SHADOWTELEPORT = 914888896U;
        static const AkUniqueID SHOOT = 3038207054U;
        static const AkUniqueID SLIDEPLATFORM = 3460734889U;
        static const AkUniqueID SPRINGPLATFORM = 2890004375U;
        static const AkUniqueID STOPALL = 3086540886U;
        static const AkUniqueID TAKEDAMAGE = 2784187423U;
        static const AkUniqueID UNPAUSE = 3412868374U;
        static const AkUniqueID WAPONDROP = 1629544899U;
        static const AkUniqueID WEAPONPICKUP = 2883364373U;
    } // namespace EVENTS

    namespace STATES
    {
        namespace BUFFSKILL
        {
            static const AkUniqueID GROUP = 1022669649U;

            namespace STATE
            {
                static const AkUniqueID NOBUFF = 1145350591U;
                static const AkUniqueID NONE = 748895195U;
                static const AkUniqueID ONBUFF = 1195699893U;
            } // namespace STATE
        } // namespace BUFFSKILL

        namespace HEALTHSTATE
        {
            static const AkUniqueID GROUP = 43109054U;

            namespace STATE
            {
                static const AkUniqueID HEALTHOK = 2965683241U;
                static const AkUniqueID LOWHEALTH = 1017222595U;
                static const AkUniqueID NONE = 748895195U;
            } // namespace STATE
        } // namespace HEALTHSTATE

    } // namespace STATES

    namespace SWITCHES
    {
        namespace FOOTSWITCH
        {
            static const AkUniqueID GROUP = 2285840829U;

            namespace SWITCH
            {
                static const AkUniqueID REGULAR = 2628937827U;
                static const AkUniqueID WALLRUN = 202094804U;
            } // namespace SWITCH
        } // namespace FOOTSWITCH

        namespace FOOTWEAPONSWITCH
        {
            static const AkUniqueID GROUP = 3119421021U;

            namespace SWITCH
            {
                static const AkUniqueID HASWEAPON = 1385920355U;
                static const AkUniqueID NOWEAPON = 1072509240U;
            } // namespace SWITCH
        } // namespace FOOTWEAPONSWITCH

        namespace PAUSESWITH
        {
            static const AkUniqueID GROUP = 5197044U;

            namespace SWITCH
            {
                static const AkUniqueID PAUSE = 3092587493U;
                static const AkUniqueID UNPAUSE = 3412868374U;
            } // namespace SWITCH
        } // namespace PAUSESWITH

        namespace WEAPONMAGZINE
        {
            static const AkUniqueID GROUP = 956060894U;

            namespace SWITCH
            {
                static const AkUniqueID EMPTY = 3354297748U;
                static const AkUniqueID HASBULLET = 3258916203U;
            } // namespace SWITCH
        } // namespace WEAPONMAGZINE

        namespace WEAPONTYPE
        {
            static const AkUniqueID GROUP = 767731869U;

            namespace SWITCH
            {
                static const AkUniqueID PISTOL = 324443136U;
                static const AkUniqueID RIFLE = 4262674733U;
                static const AkUniqueID SHOTGUN = 51683977U;
            } // namespace SWITCH
        } // namespace WEAPONTYPE

    } // namespace SWITCHES

    namespace TRIGGERS
    {
        static const AkUniqueID HELATHREGEN = 788128680U;
        static const AkUniqueID LOWHEALTH = 1017222595U;
    } // namespace TRIGGERS

    namespace BANKS
    {
        static const AkUniqueID INIT = 1355168291U;
        static const AkUniqueID MAINSOUNDBANK = 534561221U;
    } // namespace BANKS

    namespace BUSSES
    {
        static const AkUniqueID MASTER_AUDIO_BUS = 3803692087U;
    } // namespace BUSSES

    namespace AUDIO_DEVICES
    {
        static const AkUniqueID NO_OUTPUT = 2317455096U;
        static const AkUniqueID SYSTEM = 3859886410U;
    } // namespace AUDIO_DEVICES

}// namespace AK

#endif // __WWISE_IDS_H__
